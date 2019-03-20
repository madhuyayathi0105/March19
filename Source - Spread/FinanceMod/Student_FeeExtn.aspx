<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_FeeExtn.aspx.cs" Inherits="Student_FeeExtn" %>

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
    <body>
        <script type="text/javascript">
            //        function ontextchange() {
            //            var totamnt = 0.00;
            //            var totded = 0.00;

            //            var tab = document.getElementById("<%=gridViewpop.ClientID %>");
            //            var gridcontrols = tab.getElementsByTagName("Input");

            //            for (var i = 0; i < (gridcontrols.length); i++) {

            //                var addcons = document.getElementById('gridViewpop_txt_addconcess_' + i.toString());
            //                var textid = document.getElementById('gridViewpop_txt_concess_' + i.toString());
            //                var feeid = document.getElementById('gridViewpop_lbl_fee_' + i.toString());
            //                var totid = document.getElementById('gridViewpop_lbl_tot_' + i.toString());
            //                var feecode = feeid.innerHTML;

            //                if (addcons.value == "" || addcons.value == "0") {
            //                    addcons.value = "0.00";
            //                }
            //                else if (textid.value == "" || textid.value == "0") {
            //                    textid.value = "0.00";
            //                }
            //                else if (feecode == "" || feecode == "0") {
            //                    feecode = "0.00";
            //                }

            //                if (parseFloat(addcons.value) != "0.00") {
            //                    totded = parseFloat(addcons.value) + parseFloat(textid.value);
            //                }
            //                else {
            //                    totded = parseFloat(textid.value);
            //                }
            //                if (totded != "" && feecode != "0.00") {
            //                    if (parseFloat(feecode) > parseFloat(totded)) {
            //                        totid.innerHTML = "";
            //                        totamnt = parseFloat(feecode) - parseFloat(totded);
            //                    }
            //                    else {
            //                        totamnt = parseFloat(feecode) - parseFloat(textid.value);
            //                        addcons.value = "0.00";
            //                        alert("Deduct Amount should be less than Fee Amount!");
            //                    }
            //                }
            //                totid.innerHTML = totamnt;
            //            }
            //        }



            //        function changeimage() {
            //            var image = document.getElementById("");
            //            if (image.src.Match("~/images/7tnmeliv.jpg")) {
            //                image.src = "~/images/7tnmeliv.jpg";
            //            }
            //            else {
            //                image.src = "~/images/ahtb7lav.jpg";
            //            }
            //        }

            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Student Fee Concession/Extension</span>
                        <br />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div class="maindivstyle" style="width: 950px; height: auto;">
                <center>
                    <div id="div_refund" runat="server">
                        <div style="width: 900px">
                            <br />
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddl_college" Height="25px" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
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
                                                    <asp:TextBox ID="txt_rdate" runat="server" ReadOnly="true" CssClass="textbox txtheight textbox2"></asp:TextBox>
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
                                        <%--<td>
                                        <asp:Image ID="myimage" runat="server" onmousedown="return changeimage()" Visible="true"
                                            ImageUrl="~/images/ahtb7lav.jpg" Width="100px" Height="180px" />
                                        <p>
                                            To turn on/turn off the bulb by clicking the bulb!</p>
                                    </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_rename" runat="server" ReadOnly="true" CssClass="textbox txtheight6 textbox1"
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
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rebatch" runat="server" ReadOnly="true" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_str" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_restrm" runat="server" ReadOnly="true" CssClass="txtheight1 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_redegree" runat="server" ReadOnly="true" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            &nbsp;<asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txt_redept" runat="server" ReadOnly="true" CssClass="txtheight4 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_sem" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                        height: 150px;">
                                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_sem"
                                                        PopupControlID="Panel10" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_resec" runat="server" ReadOnly="true" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_showall" runat="server" Text="Show All Fees" AutoPostBack="true"
                                                OnCheckedChanged="cb_showall_Changed" ForeColor="Blue" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                Style="width: 130px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <%--*******2nd div*****--%>
                        <br />
                        <div style="float: left; width: 1000px;">
                            <center>
                                <asp:UpdatePanel ID="upd" runat="server">
                                    <ContentTemplate>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="NotSet"
                                            ShowHeaderSelection="false" BorderWidth="0px" CssClass="spreadborder" ClientAutoCalculation="true"
                                            ActiveSheetViewIndex="0" OnUpdateCommand="FpSpread1_Command">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                <table id="tblAddConc" runat="server" visible="false">
                                    <tr>
                                        <td colspan="5">
                                            <asp:CheckBox ID="chk_AddConc" runat="server" Text="Common Concession/Deduction"
                                                OnCheckedChanged="chk_AddConc_Change" AutoPostBack="true" />
                                            <%-- </td>
                                        <td>--%>
                                            <asp:DropDownList ID="ddlAddConc" runat="server" Enabled="false" CssClass="textbox1 ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <fieldset style="height: 14px;">
                                                <asp:RadioButton ID="radAmtConc" runat="server" Text="Amount" Checked="true" GroupName="RadConcGrp" />
                                                <asp:RadioButton ID="radPerConc" runat="server" Text="Percent" GroupName="RadConcGrp" />
                                            </fieldset>
                                        </td>
                                         <td colspan="2">
                                            <fieldset style="height: 14px;">
                                                <asp:RadioButtonList ID="rblconsType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Original Amt" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Already Given Amt" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAddConc" runat="server" Enabled="false" CssClass="textbox btn2"
                                                Text="Apply" OnClick="btnAddConc_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        
                                    </tr>
                                </table>
                                <br />
                                <table id="tblExpo" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                onkeypress="display()"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                                Text="Export To Excel" Width="130px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                CssClass="textbox textbox1 btn2" />
                                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnext" runat="server" CssClass="textbox textbox1 btn2" Width="160px"
                                                Text="Add Extension" OnClick="btnext_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </center>
                        </div>
                    </div>
                </center>
            </div>
            <center>
                <div id="popfine" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 12px; margin-left: 425px;" OnClick="imagepopclose_click" />
                    <br />
                    <center>
                        <div style="height: 530px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px; background-color: White;">
                            <br />
                            <br />
                            <table class="maintablestyle" width="800px">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_comdt" runat="server" OnCheckedChanged="cb_comdt_Changed" AutoPostBack="true"
                                            Text="Common Date" />
                                        <%--AutoPostBack="true"
                                        OnCheckedChanged="cb_comdt_OnCheckedChanged"--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_comdt" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                        <%--AutoPostBack="true" OnTextChanged="txt_comdt_OnTextChanged"--%>
                                        <asp:CalendarExtender ID="caldueext" runat="server" TargetControlID="txt_comdt" CssClass="cal_Theme1 ajax__calendar_active"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_comreason" runat="server" Text="Common Reason" />
                                        <%--AutoPostBack="true"
                                        OnCheckedChanged="cb_comreason_OnCheckedChanged"--%>
                                    </td>
                                    <td colspan="3" style="border: 1px solid #c4c4c4; padding: 4px 4px 4px 4px; border-radius: 4px;
                                        -moz-border-radius: 4px; -webkit-border-radius: 4px; box-shadow: 0px 0px 8px #d9d9d9;
                                        -moz-box-shadow: 0px 0px 8px #d9d9d9; -webkit-box-shadow: 0px 0px 8px #d9d9d9;">
                                        <asp:Button ID="btn_plus_detre" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            CommandName="jai" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_detre_Click" />
                                        <asp:DropDownList ID="ddl_detre" CssClass="textbox ddlheight2" runat="server">
                                            <%--AutoPostBack="true"
                                        OnSelectedIndexChanged="ddl_detre_OnSelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus_detre" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_detre_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="cbcons" runat="server" Text="Consession/Deduction" AutoPostBack="true"
                                            OnCheckedChanged="cbcons_OnCheckedChanged" GroupName="A2" />
                                        <%--  <asp:CheckBox ID="cbcons" runat="server" Text="Consession" AutoPostBack="true" OnCheckedChanged="cbcons_OnCheckedChanged" />--%>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_concesstbl" runat="server" Enabled="false" Text="Add Amount"
                                            OnCheckedChanged="cb_concesstbl_Changed" AutoPostBack="true" GroupName="g2" />
                                        <%-- <asp:CheckBox ID="cb_concesstbl" runat="server" Enabled="false" Text="Add Amount" />--%>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cbconsdecrs" runat="server" Enabled="false" Text="Decress Amount"
                                            OnCheckedChanged="cbconsdecrs_Changed" AutoPostBack="true" GroupName="g2" />
                                        <%-- <asp:CheckBox ID="cbconsdecrs" runat="server" Enabled="false" Text="Decress Amount" />--%>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cbfeeamt" runat="server" Text="FeeAmount" AutoPostBack="true"
                                            OnCheckedChanged="cbfeeamt_OnCheckedChanged" GroupName="A2" />
                                        <%-- <asp:CheckBox ID="cbfeeamt" runat="server" Text="FeeAmount" AutoPostBack="true" OnCheckedChanged="cbfeeamt_OnCheckedChanged" />--%>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cbfeeamtadd" runat="server" Enabled="false" Text="Add Amount"
                                            OnCheckedChanged="cbfeeamtadd_Changed" AutoPostBack="true" GroupName="c2" />
                                        <%-- <asp:CheckBox ID="cbfeeamtadd" runat="server" Enabled="false" Text="Add Amount" />--%>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cbfeeamtdecrs" runat="server" Enabled="false" Text="Decress Amount"
                                            OnCheckedChanged="cbfeeamtdecrs_Changed" AutoPostBack="true" GroupName="c2" />
                                        <%--  <asp:CheckBox ID="cbfeeamtdecrs" runat="server" Enabled="false" Text="Decress Amount" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <%--  <asp:CheckBox ID="cb_concesstbl" runat="server" Text="Concess Amount" />--%>
                                        <%--AutoPostBack="true"
                                        OnCheckedChanged="cb_concesstbl_OnCheckedChanged"--%>
                                        <asp:CheckBox ID="cb_datewithamnt" runat="server" Text="Date With Amount(No of Times)"
                                            AutoPostBack="true" OnCheckedChanged="cb_datewithamnt_OnCheckedChanged" />
                                        <asp:TextBox ID="txt_datewithamnt" runat="server" MaxLength="2" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtend1" runat="server" TargetControlID="txt_datewithamnt"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnadddate" runat="server" Text="Go" CssClass="textbox textbox1 btn"
                                            OnClick="btnadddate_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <asp:Label ID="lblfeeamt" runat="server" Visible="false"></asp:Label>
                            </center>
                            <center>
                                <div id="divpopup" runat="server" style="border-radius: 10px; border: 1px solid Gray;
                                    height: 250px; overflow: auto; width: 850px;">
                                    <center>
                                        <div style="height: 250px; overflow: auto;">
                                            <asp:GridView ID="gridViewpop" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                OnRowDataBound="typegrid_OnRowDataBound" OnDataBound="typebound" OnRowCommand="gridViewpop_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_yrsem" runat="server" Text='<%#Eval("sem") %>'>
                                                            </asp:Label>
                                                            <asp:Label ID="lbl_feecode" runat="server" Visible="false" Text='<%#Eval("semcode") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_hdrpop" runat="server" Text='<%#Eval("Header") %>'>
                                                            </asp:Label>
                                                            <asp:Label ID="lbl_hdridpop" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_ledge" runat="server" Text='<%#Eval("Ledger") %>'></asp:Label>
                                                                <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fee" runat="server" Text='<%#Eval("FeeAmnt") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Additional Feeamount" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtfeemat" runat="server" Style="text-align: right;" Text='<%#Eval("addfeeamt") %>'
                                                                CssClass="textbox textbox1 txtheight" Width="119px" AutoPostBack="true" OnTextChanged="txtfeemat_OnTextChanged"></asp:TextBox>
                                                            <%--AutoPostBack="true" OnTextChanged="txt_addconcess_OnTextChanged"--%>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderfeeadd" runat="server" TargetControlID="txtfeemat"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--column6 temporary txtfeeamt--%>
                                                    <asp:TemplateField HeaderText="tempfeeamt" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="tmptxtamt" runat="server"></asp:Label>
                                                            <%-- <asp:TextBox ID="tmptxtamt" runat="server" Style="text-align: right;" CssClass="textbox textbox1 txtheight"
                                                            Width="119px"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertnpadd" runat="server" TargetControlID="tmptxtamt"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Concession/Deduction" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_concess" runat="server" Enabled="false" Text='<%#Eval("concess") %>'
                                                                AutoPostBack="true" Style="text-align: right;" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrebal" runat="server" TargetControlID="txt_concess"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()"--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Additional Concession" Visible="true" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess") %>'
                                                                AutoPostBack="true" Width="119px" OnTextChanged="txt_addconcess_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd" runat="server" TargetControlID="txt_addconcess"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--column8 temporary txtfeeamt--%>
                                                    <asp:TemplateField HeaderText="tempcons" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <%--  <asp:TextBox ID="tmpconsamt" runat="server" Style="text-align: right;" CssClass="textbox textbox1 txtheight"
                                                            Width="119px"></asp:TextBox>--%>
                                                            <asp:Label ID="tmpconsamt" runat="server"></asp:Label>
                                                            <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendercons" runat="server" TargetControlID="tmpconsamt"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_tot" runat="server" Text='<%#Eval("totAmnt") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance" Visible="true" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_balamnt" runat="server" Text='<%#Eval("balamnt") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Due Extension" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ddt" runat="server" Text='<%#Eval("DDt") %>'>
                                                        </asp:Label>
                                                        <asp:TextBox ID="txt_dueext" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext_OnTextChanged"
                                                            CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                        <asp:CalendarExtender ID="popduedtext" runat="server" TargetControlID="txt_dueext"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Due Extension1" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt1" runat="server" Text='<%#Eval("DDt1") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext1" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext1_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext1" runat="server" TargetControlID="txt_dueext1"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount1" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess1" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess1") %>'
                                                                AutoPostBack="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd1" runat="server" TargetControlID="txt_addconcess1"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension2" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt2" runat="server" Text='<%#Eval("DDt2") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext2" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext2_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext2" runat="server" TargetControlID="txt_dueext2"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount2" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess2" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess2") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess2_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd2" runat="server" TargetControlID="txt_addconcess2"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension3" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt3" runat="server" Text='<%#Eval("DDt3") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext3" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext3_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext3" runat="server" TargetControlID="txt_dueext3"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount3" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess3" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess3") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess3_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd3" runat="server" TargetControlID="txt_addconcess3"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension4" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt4" runat="server" Text='<%#Eval("DDt4") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext4" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext4_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext4" runat="server" TargetControlID="txt_dueext4"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount4" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess4" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess4") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess4_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd4" runat="server" TargetControlID="txt_addconcess4"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension5" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt5" runat="server" Text='<%#Eval("DDt5") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext5" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext5_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext5" runat="server" TargetControlID="txt_dueext5"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount5" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess5" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess5") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess5_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd5" runat="server" TargetControlID="txt_addconcess5"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension6" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt6" runat="server" Text='<%#Eval("DDt6") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext6" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext6_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext6" runat="server" TargetControlID="txt_dueext6"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount6" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess6" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess6") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess6_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd6" runat="server" TargetControlID="txt_addconcess6"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension7" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt7" runat="server" Text='<%#Eval("DDt7") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext7" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext7_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext7" runat="server" TargetControlID="txt_dueext7"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount7" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess7" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess7") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess7_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd7" runat="server" TargetControlID="txt_addconcess7"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension8" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt8" runat="server" Text='<%#Eval("DDt8") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext8" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext8_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext8" runat="server" TargetControlID="txt_dueext8"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount8" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess8" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess8") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess8_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd8" runat="server" TargetControlID="txt_addconcess8"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension9" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt9" runat="server" Text='<%#Eval("DDt9") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext9" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext9_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext9" runat="server" TargetControlID="txt_dueext9"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount9" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess9" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess9") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess9_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd9" runat="server" TargetControlID="txt_addconcess9"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Extension10" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ddt10" runat="server" Text='<%#Eval("DDt10") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_dueext10" runat="server" AutoPostBack="true" OnTextChanged="txt_dueext10_OnTextChanged"
                                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="popduedtext10" runat="server" TargetControlID="txt_dueext10"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount10" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_addconcess10" runat="server" Style="text-align: right;" Text='<%#Eval("addconcess10") %>'
                                                                AutoPostBack="true" OnTextChanged="txt_addconcess10_OnTextChanged" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderadd10" runat="server" TargetControlID="txt_addconcess10"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--OnTextChanged="txt_concess_OnTextChanged" onchange="return ontextchange()" --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reason" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_reason" runat="server" Text='<%#Eval("Reason") %>'></asp:Label>
                                                            <asp:DropDownList ID="ddl_reason" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddl_reason_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </center>
                                </div>
                                <br />
                                <br />
                                <div>
                                    <asp:Button ID="btnsavepop" runat="server" Text="Save" Enabled="false" CssClass="textbox textbox1 btn2"
                                        OnClick="btnsavepop_click" />
                                    <asp:Button ID="btnextpop" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                        OnClick="btnextpop_click" />
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addreason" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addreason" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addreason" runat="server" Width="200px" CssClass="textbox textbox1"
                                            onkeypress="display1()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addreason" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_addreason_Click" />
                                        <asp:Button ID="btn_exitreason" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_exitaddreason_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="imgDiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblconfirm" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnyes" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnno" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnno_Click" Text="No" runat="server" />
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
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
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
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </center>
    </body>
    </html>
</asp:Content>
