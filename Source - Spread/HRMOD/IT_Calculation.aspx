<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="IT_Calculation.aspx.cs" Inherits="IT_Calculation" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }

            function onchck() {
                var txt = document.getElementById('<%=txt_citypercent.ClientID %>').value;
                if (parseFloat(txt) > 100) {
                    document.getElementById('<%=txt_citypercent.ClientID %>').value = "";
                }
            }

            function chkamnt(val) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(val.value) == false) {
                    val.value = "";
                }
            }

            function chkdedamnt(val) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(val.value) == false) {
                    val.value = "";
                }
            }

            function chksetsecamnt(val) {
                var ddldedval = document.getElementById('<%=ddlamntorpersec.ClientID %>').value;
                var txtfrmamnt = document.getElementById('<%=txtfrmsecamnt.ClientID %>').value;
                var txttoamnt = document.getElementById('<%=txttosecamnt.ClientID %>').value;
                var amountorper = document.getElementById('<%=txtuptosec.ClientID %>').value;
                if (ddldedval.trim() != "") {
                    if (ddldedval.trim() == "0") {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(val.value) == false) {
                            val.value = "";
                        }
                        if (amountorper.trim() != "") {
                            if (txtfrmamnt.trim() == "" || txttoamnt.trim() == "") {
                                document.getElementById('<%=txtuptosec.ClientID %>').value = "";
                                alert("Please Enter the From/To Amount!");
                            }
                            else {
                                if (parseFloat(txtfrmamnt) > parseFloat(txttoamnt)) {
                                    document.getElementById('<%=txtuptosec.ClientID %>').value = "";
                                    document.getElementById('<%=txtfrmsecamnt.ClientID %>').value = "";
                                    document.getElementById('<%=txttosecamnt.ClientID %>').value = "";
                                    alert("From Amount Should be greater than To Amount!");
                                }
                                if (parseFloat(txttoamnt) < parseFloat(amountorper)) {
                                    document.getElementById('<%=txtuptosec.ClientID %>').value = "";
                                    alert("Tax Amount Should be less than To Amount!");
                                }
                            }
                        }
                    }
                    else if (ddldedval.trim() == "1") {
                        if (val.value.trim() != "") {
                            if (parseFloat(val.value) > 100) {
                                val.value = "";
                            }
                            if (amountorper.trim() != "") {
                                if (txtfrmamnt.trim() == "" || txttoamnt.trim() == "") {
                                    document.getElementById('<%=txtuptosec.ClientID %>').value = "";
                                    alert("Please Enter the From/To Amount!");
                                }
                                else {
                                    if (parseFloat(txtfrmamnt) > parseFloat(txttoamnt)) {
                                        document.getElementById('<%=txtfrmsecamnt.ClientID %>').value = "";
                                        document.getElementById('<%=txttosecamnt.ClientID %>').value = "";
                                        document.getElementById('<%=txtuptosec.ClientID %>').value = "";
                                        alert("From Amount Should be greater than To Amount!");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            function check() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=txtadddedgrp.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txtadddedgrp.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                var newid = "";
                var newempty = "";
                newid = document.getElementById("<%=txtdedgrpamnt.ClientID %>").value;
                if (newid.trim() == "") {
                    newid = document.getElementById("<%=txtdedgrpamnt.ClientID %>");
                    newid.style.borderColor = 'Red';
                    newempty = "E";
                }

                if (newempty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function getdedgrpname(txt) {
                $.ajax({
                    type: "POST",
                    url: "IT_Calculation.aspx/checkdedgrpname",
                    data: '{dedgrpname: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: onSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function onSuccess(response) {
                var mesg1 = $("#screrracr")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "";
                        break;
                    case "1":
                        mesg1.style.color = "red";
                        document.getElementById('<%=txtadddedgrp.ClientID %>').value = "";
                        mesg1.innerHTML = "Already Exist!";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter Deduction Group Name!";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error occurred";
                        break;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Income Tax Calculation</span></div>
                </center>
                <fieldset id="maindiv" runat="server" style="width: 978px; height: 1100px; border-color: silver;
                    border-radius: 10px;">
                    <fieldset style="height: 122px; width: 960px; border: 1px solid #0ca6ca; border-radius: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College Name : " Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="120px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="356px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_scode" runat="server" Text="Staff Code" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                                        CssClass="textbox textbox1" MaxLength="10" Style="font-weight: bold; width: 100px;
                                        font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                                        CssClass="textbox textbox1" MaxLength="50" Style="font-weight: bold; font-family: book antiqua;
                                        font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                        </table>
                        <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                            box-shadow: 0 0 8px #999999; height: 45px; margin-left: 0px; margin-top: 8px;
                            padding: 1em; margin-left: 0px; width: 924px;">
                            <table style="margin-top: -14px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="200px">
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
                                        <asp:Label ID="lbl_desig" runat="server" Text="Designation" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P2" runat="server" CssClass="multxtpanel" Height="200px">
                                                    <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_desig"
                                                    PopupControlID="P2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P3" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                                    <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                                    PopupControlID="P3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stype" runat="server" Text="Staff Type" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P4" runat="server" CssClass="multxtpanel" Height="200px">
                                                    <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                                    PopupControlID="P4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_stat" runat="server" Text="Staff Status" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stat" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P5" runat="server" CssClass="multxtpanel" Height="200px">
                                                    <asp:CheckBox ID="cb_stat" runat="server" Text="Select All" OnCheckedChanged="cb_stat_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_stat" runat="server" OnSelectedIndexChanged="cbl_stat_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stat"
                                                    PopupControlID="P5" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="5">
                                        <asp:LinkButton ID="lnkitsetting" runat="server" Text="IT Calculation Settings" OnClick="lnkitsetting_click"
                                            Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua"></asp:LinkButton>
                                        <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" CssClass="textbox textbox1 btn2"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                                        <asp:Button ID="btn_gp" runat="server" Text="Set Details" OnClick="btn_st_Click"
                                            CssClass="textbox textbox1 btn2" Width="104px" Style="font-weight: bold; font-family: book antiqua;
                                            font-size: medium;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </fieldset>
                    </br>
                    <center>
                        <asp:Label ID="lbl_alert" runat="server" Visible="false" Style="color: red; font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </center>
                    <br />
                    <div id="sp_div" runat="server">
                        <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="980px" Height="800px" Style="margin-left: 2px;"
                            class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div id="rprint" runat="server" visible="false">
                            <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                                Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2" Width="100px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                </fieldset>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 464px;"
                    OnClick="imagebtnpopcloseadd_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 85px; overflow: auto;
                    width: 890px; height: 963px;" align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 890px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbaddinc" runat="server" OnCheckedChanged="cbaddinc_CheckedChange"
                                        Style="margin-left: 20px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Additional Income" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_addinc" runat="server" Text="Add" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" CssClass="textbox textbox1 btn2" OnClick="btn_addinc_click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div id="addinc_div" runat="server" visible="false" style="border: 2px solid indigo;
                                border-radius: 10px; height: 307px; width: 615px;">
                                <br />
                                <center>
                                    <asp:Label ID="lbl_addincome" runat="server" Text="Additional Income" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                </center>
                                <br />
                                <center>
                                    <table>
                                        <tr>
                                            <td colspan="3" style="border: 1px solid #c4c4c4; background-color: #0CA6CA; padding: 4px 4px 4px 4px;
                                                border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; box-shadow: 0px 0px 8px #c4c4c4;
                                                -moz-box-shadow: 0px 0px 8px #c4c4c4; -webkit-box-shadow: 0px 0px 8px #c4c4c4;">
                                                Income Head
                                                <asp:Button ID="btn_plus_detre" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                    CommandName="jai" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_detre_Click" />
                                                <asp:DropDownList ID="ddl_detre" CssClass="textbox1 ddlheight2" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_minus_detre" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_detre_Click" />
                                            </td>
                                            <td style="padding: 10px">
                                                Doc No
                                                <asp:TextBox ID="txtdocno" runat="server" MaxLength="15" Width="135px" CssClass="textbox textbox1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Doc Date
                                                <asp:TextBox ID="txtdocdate" runat="server" CssClass="textbox textbox1" Width="119px"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_docdate" runat="server" TargetControlID="txtdocdate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="padding: 10px">
                                                Amount
                                                <asp:TextBox ID="txtamount" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                                    CssClass="textbox textbox1" Width="135px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Cheque No
                                                <asp:TextBox ID="txtchqno" runat="server" MaxLength="15" CssClass="textbox textbox1"
                                                    Width="107px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Cheque Date
                                                <asp:TextBox ID="txtchqdt" runat="server" CssClass="textbox textbox1" Width="112px"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_chqdt" runat="server" TargetControlID="txtchqdt" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_allowalert" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                                </center>
                                <br />
                                <center>
                                    <asp:Button ID="btn_allincsave" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btn_allincsave_Click" Text="Add" CssClass="textbox1 textbox btn2" />
                                    <asp:Button ID="btn_allincupdate" runat="server" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" OnClick="btn_allincupdate_Click" Text="Update" CssClass="textbox1 textbox btn2" />
                                    <asp:Button ID="btn_allincdelete" runat="server" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" OnClick="btn_allincdelete_Click" Text="Delete" CssClass="textbox1 textbox btn2" />
                                    <asp:Button ID="btn_allincexit" runat="server" OnClick="btn_allincexit_Click" Font-Bold="true"
                                        Font-Names="Book Antiqua" Text="Exit" CssClass="textbox1 textbox btn2" />
                                </center>
                                <br />
                            </div>
                            <div id="divgrdaddinc" runat="server" visible="false" style="border: 2px solid indigo;
                                border-radius: 10px; height: 300px; margin-left: 27px; width: 650px;">
                                <div style="height: 250px; overflow: auto;">
                                    <center>
                                        <asp:GridView ID="grd_addinc" runat="server" AutoGenerateColumns="false" Visible="false"
                                            GridLines="Both" OnRowDataBound="grd_addinc_rowbound" OnRowCommand="grd_addinc_rowcommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Asst Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allassyear" runat="server" Text='<%#Eval("allassyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Income Head" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_inchead" runat="server" Text='<%#Eval("inchead") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IT Mon/Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allitmonyear" runat="server" Text='<%#Eval("allitmonyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="A/C Mon/Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allacmonyear" runat="server" Text='<%#Eval("allacmonyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allamnt" runat="server" Text='<%#Eval("allamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_alldocno" runat="server" Text='<%#Eval("alldocno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_alldocdate" runat="server" Text='<%#Eval("alldocdate") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cheque No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allchqno" runat="server" Text='<%#Eval("allchqno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cheque Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_allchqdt" runat="server" Text='<%#Eval("allchqdt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </center>
                                </div>
                            </div>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbdeduction" runat="server" OnCheckedChanged="cbdeduction_CheckedChange"
                                        Style="margin-left: 20px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Deduction" />
                                </td>
                                <td>
                                    <asp:Button ID="btnadddeduction" runat="server" Text="Add" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" CssClass="textbox textbox1 btn2" OnClick="btnadddeduction_click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div id="deduction_div" runat="server" visible="false" style="border: 2px solid indigo;
                                border-radius: 10px; height: 355px; width: 615px;">
                                <br />
                                <center>
                                    <asp:Label ID="lbldeduction" runat="server" Text="Deduction" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                </center>
                                <br />
                                <center>
                                    <table>
                                        <tr>
                                            <td colspan="3" style="border: 1px solid #c4c4c4; background-color: #0CA6CA; padding: 4px 4px 4px 4px;
                                                border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; box-shadow: 0px 0px 8px #d9d9d9;
                                                -moz-box-shadow: 0px 0px 8px #d9d9d9; -webkit-box-shadow: 0px 0px 8px #d9d9d9;">
                                                Deduction
                                                <asp:Button ID="btndedplus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                    CommandName="jp" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btndedplus_detre_Click" />
                                                <asp:DropDownList ID="ddldedhead" CssClass="textbox1 ddlheight2" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:Button ID="btndedmin" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btndedmin_detre_Click" />
                                            </td>
                                            <td style="padding: 10px">
                                                Deduction Group
                                                <asp:DropDownList ID="ddldedgrp" runat="server" CssClass="textbox1 ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Doc No
                                                <asp:TextBox ID="txtdeddocno" runat="server" MaxLength="15" Width="152px" CssClass="textbox textbox1"></asp:TextBox>
                                            </td>
                                            <td style="padding: 10px">
                                                Doc Date
                                                <asp:TextBox ID="txtdeddocdate" runat="server" CssClass="textbox textbox1" Width="119px"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_deddt" runat="server" TargetControlID="txtdeddocdate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Amount
                                                <asp:TextBox ID="txtdedamnt" runat="server" MaxLength="15" onkeyup="chkdedamnt(this);"
                                                    CssClass="textbox textbox1" Width="153px"></asp:TextBox>
                                            </td>
                                            <td style="padding: 10px">
                                                Cheque No
                                                <asp:TextBox ID="txtdedchqno" runat="server" MaxLength="15" CssClass="textbox textbox1"
                                                    Width="107px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cheque Date
                                                <asp:TextBox ID="txtdedchqdt" runat="server" CssClass="textbox textbox1" Width="121px"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_dedchqdt" runat="server" TargetControlID="txtdedchqdt"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <center>
                                    <asp:Label ID="lbldederr" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                                </center>
                                <br />
                                <center>
                                    <asp:Button ID="btndedsave" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btndedsave_Click" Text="Add" CssClass="textbox textbox1 btn2" />
                                    <asp:Button ID="btndedupdate" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btndedupdate_Click" Text="Update" CssClass="textbox textbox1 btn2" />
                                    <asp:Button ID="btndeddelete" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btndeddelete_Click" Text="Delete" CssClass="textbox textbox1 btn2" />
                                    <asp:Button ID="btndedexit" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btndedexit_Click" Text="Exit" CssClass="textbox textbox1 btn2" />
                                </center>
                                <br />
                            </div>
                            <div id="divgrdded" runat="server" visible="false" style="border: 2px solid indigo;
                                border-radius: 10px; height: 300px; margin-left: 27px; width: 650px;">
                                <div style="height: 250px; overflow: auto;">
                                    <center>
                                        <asp:GridView ID="grdded" runat="server" AutoGenerateColumns="false" Visible="false"
                                            GridLines="Both" OnRowDataBound="grdded_rowbound" OnRowCommand="grdded_rowcommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Asst Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedassyear" runat="server" Text='<%#Eval("dedassyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deduction Head" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedinchead" runat="server" Text='<%#Eval("dedinchead") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IT Mon/Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_deditmonyear" runat="server" Text='<%#Eval("deditmonyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="A/C Mon/Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedacmonyear" runat="server" Text='<%#Eval("dedacmonyear") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedamnt" runat="server" Text='<%#Eval("dedamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_deddocno" runat="server" Text='<%#Eval("deddocno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_deddocdate" runat="server" Text='<%#Eval("deddocdate") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cheque No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedchqno" runat="server" Text='<%#Eval("dedchqno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cheque Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedchqdt" runat="server" Text='<%#Eval("dedchqdt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deduction Group" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_dedgrp" runat="server" Text='<%#Eval("dedgrp") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </center>
                                </div>
                            </div>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btnsaveincome" runat="server" CssClass="textbox textbox1 btn2" Width="150px" Font-Bold="true"
                                Font-Names="Book Antiqua" Text="Set Income Details" OnClick="btnsaveincome_click" />
                            <asp:Button ID="btnexitincome" runat="server" CssClass="textbox textbox1 btn2" Font-Bold="true"
                                Font-Names="Book Antiqua" Text="Exit" OnClick="btnexitincome_click" />
                        </center>
                        <br />
                    </div>
                </div>
            </div>
            <div id="divitcalset" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="imgitset" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 32px; margin-left: 446px;"
                    OnClick="imgitsetpopcloseadd_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 920px;
                    height: 650px;" align="center">
                    <br />
                    <div align="left" style="overflow: auto; width: 893px; height: 560px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <asp:Label ID="lblitcalset" runat="server" Text="IT Calculation Settings" Style="font-weight: bold;
                                font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        From Month & Year
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlfrmmon" runat="server" OnSelectedIndexChanged="ddlfrmmon_Change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight">
                                            <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlfrmyear" runat="server" OnSelectedIndexChanged="ddlfrmyear_change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        To Month & Year
                                        <asp:DropDownList ID="ddltomon" runat="server" OnSelectedIndexChanged="ddltomon_Change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight">
                                            <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddltoyear" runat="server" OnSelectedIndexChanged="ddltoyear_change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        From Rs.
                                        <asp:TextBox ID="txtfrmsecamnt" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                            CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterfrmsecamnt" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtfrmsecamnt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        to Rs.<asp:TextBox ID="txttosecamnt" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                            OnTextChanged="txttosecamnt_change" AutoPostBack="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtertosecamnt" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="." TargetControlID="txttosecamnt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlamntorpersec" runat="server" OnSelectedIndexChanged="ddlamntorpersec_change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight">
                                            <asp:ListItem Selected="True" Text="Amount" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Percent" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtuptosec" runat="server" MaxLength="8" onkeyup="chksetsecamnt(this);"
                                            CssClass="textbox textbox1" Width="125px" Height="19px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtersec" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtuptosec">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnadditset" runat="server" CssClass="textbox textbox1 btn2" Text="Add"
                                            OnClick="btnadditset_click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Gender
                                        <asp:DropDownList ID="ddlgender" runat="server" OnSelectedIndexChanged="ddlgender_change"
                                            AutoPostBack="false" CssClass="textbox1 ddlheight2">
                                            <asp:ListItem Selected="True" Text="Male" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Female" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="TransGender" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Button ID="btnview" runat="server" Visible="false" Text="View" OnClick="btnview_click"
                                    CssClass="textbox textbox1 btn2" />
                            </center>
                            <div id="divgrditset" runat="server" visible="false" style="border: 2px solid indigo;
                                border-radius: 10px; height: 250px; margin-left: 27px; width: 600px;">
                                <div style="height: 200px; overflow: auto;">
                                    <center>
                                        <asp:GridView ID="grditset" runat="server" AutoGenerateColumns="false" Visible="false"
                                            GridLines="Both" OnRowDataBound="grditset_rowbound" OnRowCommand="grditset_rowcommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("sno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_frmamnt" runat="server" Text='<%#Eval("itfrmamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_toamnt" runat="server" Text='<%#Eval("ittoamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_itmode" runat="server" Text='<%#Eval("itmode") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amt/Per" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_itamntorper" runat="server" Text='<%#Eval("itamntorper") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gender" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_gender" runat="server" Text='<%#Eval("gender") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </center>
                                </div>
                                <center>
                                    <asp:Button ID="btnsaveitset" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                        Text="Save" OnClick="btnsaveitset_Click" />
                                    <asp:Button ID="btnupdategrd" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                        Text="Update" OnClick="btnupdategrd_Click" />
                                    <asp:Button ID="btndeletegrd" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                        Text="Delete" OnClick="btndeletegrd_Click" />
                                    <asp:Button ID="btnexititset" runat="server" CssClass="textbox textbox1 btn2" Text="Hide"
                                        OnClick="btnexititset_Click" />
                                </center>
                            </div>
                            <br />
                            <center>
                                <asp:Button ID="btnsaveallitset" runat="server" Text="Set IT Calculation Settings"
                                    CssClass="textbox textbox1 btn2" Width="220px" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnsaveallitset_Click" />
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="lblyearval" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            Deduction Group
                                        </td>
                                        <td style="float: left;">
                                            <asp:Button ID="btndedgrpadd" runat="server" Text="+" CssClass="textbox textbox1 btn"
                                                OnClick="btndedgrpadd_click" />
                                        </td>
                                        <td style="float: left;">
                                            <asp:UpdatePanel ID="upddedgrp" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtdedgrp" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnldedgrp" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                        <asp:CheckBox ID="chkdedgrp" runat="server" AutoPostBack="true" OnCheckedChanged="chkdedgrp_changed"
                                                            Text="Select All" />
                                                        <asp:CheckBoxList ID="chklstdedgrp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstdedgrp_changed" />
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popdedgrp" runat="server" TargetControlID="txtdedgrp"
                                                        PopupControlID="pnldedgrp" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="float: left;">
                                            <asp:Button ID="btndedgrpdel" runat="server" Text="-" Visible="false" CssClass="textbox textbox1 btn"
                                                OnClick="btndedgrpdel_click" />
                                        </td>
                                        <td style="float: left;">
                                            <asp:Button ID="btngodedgrp" runat="server" Text="GO" CssClass="textbox textbox1 btn1"
                                                OnClick="btngodedgrp_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="grddedgrp" runat="server" AutoGenerateColumns="false" Visible="false"
                                                GridLines="Both" OnRowDataBound="grddedgrp_rowbound" OnRowCommand="grddedgrp_rowcommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("sno") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deduction Group" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_dedgrpname" runat="server" Text='<%#Eval("dedgrp") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbl_dedgrpid" runat="server" Visible="false" Text='<%#Eval("dedgrpid") %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Max Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_maxdedamnt" runat="server" Text='<%#Eval("maxdedamnt") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <center>
                                <div style="width: 875px;">
                                    <div style="width: 420px; float: left;">
                                        <center>
                                            <asp:Label ID="lbl_commonheader" runat="server" Text="Common Allowance" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                        </center>
                                        <br />
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span>Allowance Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upp4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_allowancemultiple" runat="server" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                                    <asp:CheckBox ID="cb_allowancemultiple" runat="server" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_allowancemultiple_checkedchange" />
                                                                    <asp:CheckBoxList ID="cbl_allowancemultiple" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_allowancemultiple_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_allowancemultiple"
                                                                    PopupControlID="Panel1" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_allowance_go" Text="Go" CssClass="textbox btn1" runat="server"
                                                            OnClick="btn_allowance_go_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Label ID="lbl_errorallowan" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                                            <center>
                                                <FarPoint:FpSpread ID="FpSpread2" Visible="false" runat="server" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="1px" Width="416px" Height="300px" Style="overflow: auto;"
                                                    CssClass="spreadborder">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                            <br />
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:CheckBox ID="cb_includecity" runat="server" AutoPostBack="true" OnCheckedChanged="cb_includecity_change"
                                                                Text="Include City" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span>City</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_city" runat="server" ReadOnly="true" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                                        <asp:CheckBox ID="cb_city" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_city_checkedchange" />
                                                                        <asp:CheckBoxList ID="cbl_city" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_city_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_city"
                                                                        PopupControlID="Panel2" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <span>Percentage</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_citypercent" runat="server" MaxLength="3" CssClass="textbox txtheight"
                                                                onblur="return onchck()" onkeyup="return onchck()"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterpercent" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_citypercent">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <center>
                                                    <asp:Button ID="btn_allowancesave" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_allowancesave_Click" />
                                                </center>
                                                <br />
                                            </center>
                                        </center>
                                    </div>
                                    <div style="width: 420px; float: right;">
                                        <center>
                                            <asp:Label ID="Label1" runat="server" Text="Common Deduction" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                        </center>
                                        <br />
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span>Deduction Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_deduction" runat="server" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                                    <asp:CheckBox ID="cb_deduction" runat="server" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_deduction_checkedchange" />
                                                                    <asp:CheckBoxList ID="cbl_deduction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_deduction_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_deduction"
                                                                    PopupControlID="Panel3" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_deduction" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_deduction_go_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Label ID="lbl_deductionerror" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                                            <center>
                                                <FarPoint:FpSpread ID="FpSpread3" Visible="false" runat="server" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="1px" Width="317px" Height="300px" Style="overflow: auto;"
                                                    CssClass="spreadborder">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                        </center>
                                        <br />
                                        <center>
                                            <asp:Button ID="btn_deductionsave" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_deductionsave_Click" />
                                            <asp:Button ID="btn_exitduc" runat="server" Visible="false" Text="Exit" CssClass="textbox textbox1 btn2"
                                                Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnexititallset_Click" />
                                        </center>
                                        <br />
                                    </div>
                                </div>
                            </center>
                            <br />
                        </center>
                    </div>
                    <br />
                    <center>
                        <asp:Button ID="btnexititallset" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnexititallset_Click" />
                    </center>
                </div>
            </div>
            <div id="divdedgrppop" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 85px; overflow: auto;
                    width: 440px; height: 285px;" align="center">
                    <br />
                    <div align="left" style="overflow: auto; width: 400px; height: 245px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <asp:Label ID="lbldedgrp" runat="server" Text="Add Deduction Group" Style="font-weight: bold;
                                font-family: book antiqua; font-size: medium; color: green;"></asp:Label>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        Deduction Group
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtadddedgrp" runat="server" CssClass="textbox textbox1 txtheight4"
                                            onblur="return getdedgrpname(this.value)" onfocus="return myFunction(this)" MaxLength="25"></asp:TextBox>
                                        <span style="font-weight: bold; font-size: medium; color: Red;" id="Span1">*</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtadddedgrp"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" InvalidChars="!@$%^&()_+}{][';,."
                                            ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdedgrpamnt" runat="server" CssClass="textbox textbox1 txtheight4"
                                            onfocus="return myFunction(this)" MaxLength="10"></asp:TextBox>
                                        <asp:TextBox ID="txtdedid" runat="server" Visible="false"></asp:TextBox>
                                        <span style="font-weight: bold; font-size: medium; color: Red;" id="Span2">*</span>
                                        <asp:FilteredTextBoxExtender ID="filteramnt" runat="server" FilterType="Numbers,custom"
                                            ValidChars="." TargetControlID="txtdedgrpamnt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btn_adddedgrp" runat="server" Visible="false" Text="Add" CssClass="textbox textbox1 btn2"
                                OnClick="btn_adddedgrp_Click" OnClientClick="return check()" />
                            <asp:Button ID="btn_upddedgrp" runat="server" Visible="false" Text="Update" CssClass="textbox textbox1 btn2"
                                OnClick="btn_upddedgrp_Click" />
                            <asp:Button ID="btn_deldedgrp" runat="server" Visible="false" Text="Delete" CssClass="textbox textbox1 btn2"
                                OnClick="btn_deldedgrp_Click" />
                            <asp:Button ID="btnexitdedgrp" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                OnClick="btnexitdedgrp_Click" />
                            <br />
                            <br />
                            <span style="font-weight: bold; font-size: larger;" id="screrracr"></span>
                        </center>
                        <br />
                    </div>
                </div>
            </div>
            <div id="divyearpop" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="imgyear" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 380px;"
                    OnClick="imgyear_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 85px; overflow: auto;
                    width: 721px; height: 328px;" align="center">
                    <br />
                    <div align="left" style="overflow: auto; width: 676px; height: 286px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <asp:Label ID="lblyearsel" runat="server" Text="Select Year" Style="font-weight: bold;
                                font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <fieldset style="width: 595px; height: 90px; border-color: Black; border-radius: 10px;
                                background-color: #0CA6CA;">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            Accessment From Year
                                            <asp:DropDownList ID="ddl_accfrmyear" runat="server" OnSelectedIndexChanged="ddl_accfrmyear_Change"
                                                AutoPostBack="true" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="3">
                                            Accessment To Year
                                            <asp:DropDownList ID="ddl_acctoyear" runat="server" OnSelectedIndexChanged="ddl_acctoyear_Change"
                                                AutoPostBack="true" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 10px" colspan="3">
                                            IT Mon & Year
                                            <asp:DropDownList ID="ddl_itmon" runat="server" CssClass="textbox1 ddlheight">
                                                <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_ityear" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="3">
                                            A/C Mon & Year
                                            <asp:DropDownList ID="ddl_accmon" runat="server" CssClass="textbox1 ddlheight">
                                                <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_accyear" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </center>
                        <br />
                        <center>
                            <asp:Label ID="lblerryear" runat="server" Text="" Visible="false" Font-Bold="true"
                                Font-Size="Large" Font-Names="Book Antiqua" ForeColor="Red"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btnokyear" runat="server" Text="OK" OnClick="btnokyear_click" CssClass="textbox textbox1 btn2" />
                            <asp:Button ID="btnexityear" runat="server" Text="Exit" OnClick="btnexityear_click"
                                CssClass="textbox textbox1 btn2" />
                        </center>
                        <br />
                    </div>
                </div>
            </div>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addreason" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addreason" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addreason" runat="server" MaxLength="20" Width="200px" CssClass="textbox textbox1"
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
    </body>
    </html>
</asp:Content>
