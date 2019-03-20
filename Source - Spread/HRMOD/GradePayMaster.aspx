<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="GradePayMaster.aspx.cs" Inherits="GradePayMaster" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }

            function checkamnt(el) {
                var ddlval = document.getElementById("<%=ddl_mode.ClientID %>").value;
                if (ddlval.trim() != "") {
                    if (ddlval.trim() == "Amount") {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(el.value) == false) {
                            el.value = "";
                        }
                    }
                    else if (ddlval.trim() == "Percent") {
                        if (el.value.trim() != "") {
                            if (parseFloat(el.value) > 1000) {
                                el.value = "";
                            }
                        }
                    }
                    else {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(el.value) == false) {
                            el.value = "";
                        }
                    }
                }
            }

            function checkoverallamnt(el) {
                var ddloverval = document.getElementById("<%=ddloverallmode.ClientID %>").value;
                if (ddloverval.trim() != "") {
                    if (ddloverval.trim() == "Amount") {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(el.value) == false) {
                            el.value = "";
                        }
                    }
                    else if (ddloverval.trim() == "Percent") {
                        if (el.value.trim() != "") {
                            if (parseFloat(el.value) > 1000) {
                                el.value = "";
                            }
                        }
                    }
                    else {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(el.value) == false) {
                            el.value = "";
                        }
                    }
                }
            }

            function chkleave() {
                var txtyl = document.getElementById("<%=txt_yl.ClientID %>").value;
                var txtml = document.getElementById("<%=txt_ml.ClientID %>").value;
                if (txtyl.trim() != "" || txtml.trim() != "") {
                    if (parseInt(txtyl) != 0) {
                        if (parseInt(txtyl) > 366) {
                            document.getElementById("<%=txt_yl.ClientID %>").value = "";
                            alert("Year Leave Exceeds the Limit!");
                        }
                    }
                    if (parseInt(txtyl) != 0 && parseInt(txtml) != 0) {
                        if (parseInt(txtml) > 31) {
                            document.getElementById("<%=txt_ml.ClientID %>").value = "";
                            document.getElementById("<%=txt_yl.ClientID %>").value = "";
                            alert("Month Leave Exceeds the Limit!");
                        }
                        if (parseInt(txtyl) < parseInt(txtml)) {
                            document.getElementById("<%=txt_ml.ClientID %>").value = "";
                            document.getElementById("<%=txt_yl.ClientID %>").value = "";
                            alert("Year Leave should be greater than Month Leave!");
                        }
                    }
                }
            }

            function chkoverleave() {
                var txtyl = document.getElementById("<%=txtoveryrlev.ClientID %>").value;
                var txtml = document.getElementById("<%=txtovermonlev.ClientID %>").value;
                if (txtyl.trim() != "" || txtml.trim() != "") {
                    if (parseInt(txtyl) != 0) {
                        if (parseInt(txtyl) > 366) {
                            document.getElementById("<%=txtoveryrlev.ClientID %>").value = "";
                            alert("Year Leave Exceeds the Limit!");
                        }
                    }
                    if (parseInt(txtyl) != 0 && parseInt(txtml) != 0) {
                        if (parseInt(txtml) > 31) {
                            document.getElementById("<%=txtovermonlev.ClientID %>").value = "";
                            document.getElementById("<%=txtoveryrlev.ClientID %>").value = "";
                            alert("Month Leave Exceeds the Limit!");
                        }
                        if (parseInt(txtyl) < parseInt(txtml)) {
                            document.getElementById("<%=txtovermonlev.ClientID %>").value = "";
                            document.getElementById("<%=txtoveryrlev.ClientID %>").value = "";
                            alert("Year Leave should be greater than Month Leave!");
                        }
                    }
                }
            }

            function chkamntded(fl) {
                var ddlval = document.getElementById("<%=ddl_dmode.ClientID %>").value;
                if (ddlval.trim() != "") {
                    if (ddlval.trim() == "Amount") {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(fl.value) == false) {
                            fl.value = "";
                        }
                    }
                    else if (ddlval.trim() == "Percent") {
                        if (fl.value.trim() != "") {
                            if (parseFloat(fl.value) > 1000) {
                                fl.value = "";
                            }
                        }
                    }
                    else {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(fl.value) == false) {
                            fl.value = "";
                        }
                    }
                }
            }

            function chkamntoverded(fl) {
                var ddlval = document.getElementById("<%=ddloverdedmode.ClientID %>").value;
                if (ddlval.trim() != "") {
                    if (ddlval.trim() == "Amount") {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(fl.value) == false) {
                            fl.value = "";
                        }
                    }
                    else if (ddlval.trim() == "Percent") {
                        if (fl.value.trim() != "") {
                            if (parseFloat(fl.value) > 1000) {
                                fl.value = "";
                            }
                        }
                    }
                    else {
                        var ex = /^[0-9]+\.?[0-9]*$/;
                        if (ex.test(fl.value) == false) {
                            fl.value = "";
                        }
                    }
                }
            }

            function chkdoubleamnt(eve) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(eve.value) == false) {
                    eve.value = "";
                }
            }

            function percent() {
                var txtval = document.getElementById("<%=txtismpfper.ClientID %>").value;
                if (txtval.trim() != "") {
                    if (parseFloat(txtval) > 100) {
                        document.getElementById('<%=txtismpfper.ClientID %>').value = "";
                    }
                }
            }


            function overallpercent() {
                var txtval = document.getElementById("<%=txtismpfperover.ClientID %>").value;
                if (txtval.trim() != "") {
                    if (parseFloat(txtval) > 100) {
                        document.getElementById('<%=txtismpfperover.ClientID %>').value = "";
                    }
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">GradePayMaster</span></div>
                </center>
                <fieldset id="maindiv" runat="server" style="width: 978px; margin-left: 0px; height: 1100px;
                    border-color: silver; border-radius: 10px;">
                    <fieldset style="height: 130px; width: 960px; border: 1px solid #0ca6ca; border-radius: 10px;">
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
                                        margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                                        MaxLength="10" Style="font-weight: bold; width: 100px; margin-left: 10px; font-family: book antiqua;
                                        font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                        margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                                        MaxLength="50" Style="font-weight: bold; font-family: book antiqua; margin-left: 0px;
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
                            box-shadow: 0 0 8px #999999; height: 50px; margin-left: 0px; margin-top: 8px;
                            padding: 1em; margin-left: 0px; width: 930px;">
                            <table style="margin-top: -14px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                    width: 250px;">
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
                                                <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                    width: 250px;">
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
                                                <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                                <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                                <asp:TextBox ID="txt_stat" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P5" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                    <td>
                                        <asp:CheckBox ID="chk_allvis" runat="server" OnCheckedChanged="chk_allvis_change"
                                            AutoPostBack="true" />
                                        <asp:Label ID="lbl_allow" runat="server" Text="Allowance" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_allow" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Enabled="false" Style="font-weight: bold; width: 120px; font-family: book antiqua;
                                                    font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P6" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                    width: 250px;">
                                                    <asp:CheckBox ID="cb_allow" runat="server" Text="Select All" OnCheckedChanged="cb_allow_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_allow" runat="server" OnSelectedIndexChanged="cbl_allow_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_allow"
                                                    PopupControlID="P6" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_dedvis" runat="server" OnCheckedChanged="chk_dedvis_change"
                                            AutoPostBack="true" />
                                        <asp:Label ID="lbl_deduct" runat="server" Text="Deduction" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_deduct" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Enabled="false" Style="font-weight: bold; width: 120px; font-family: book antiqua;
                                                    font-size: medium;">--Select--</asp:TextBox>
                                                <asp:Panel ID="P7" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                    width: 250px;">
                                                    <asp:CheckBox ID="cb_deduction" runat="server" Text="Select All" OnCheckedChanged="cb_deduction_CheckedChange"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_deduction" runat="server" OnSelectedIndexChanged="cbl_deduction_SelectedIndexChange"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_deduct"
                                                    PopupControlID="P7" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" />
                                        <asp:Button ID="btn_gp" runat="server" Text="Set GradePay" OnClick="btn_gp_Click"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
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
                            class="spreadborder" OnButtonCommand="FpSpread_Command" ShowHeaderSelection="false">
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
                                Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="140px" Text="Export Excel"
                                OnClick="btnexcel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                                Width="100px" />
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
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 440px;"
                    OnClick="imagebtnpopcloseadd_Click" />
                <br />
                <br />
                <div id="Div1" class="subdivstyle" runat="server" style="background-color: White;
                    margin-left: 85px; overflow: auto; width: 830px; height: 1340px;" align="center">
                    <br />
                    <br />
                    <table id="Table1" class="maintablestyle" runat="server">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdbmainind" runat="server" OnCheckedChanged="rdbmainind_change"
                                    AutoPostBack="true" Text="Individual" Checked="true" GroupName="div" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdbmainoverall" runat="server" OnCheckedChanged="rdbmainoverall_change"
                                    AutoPostBack="true" Text="Overall" GroupName="div" />
                                <asp:DropDownList ID="ddloverall" runat="server" OnSelectedIndexChanged="ddloverall_change"
                                    AutoPostBack="true" CssClass="textbox1 ddlheight3" Enabled="false">
                                    <asp:ListItem Selected="True" Text="Allowance" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Deduction" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Leave Type" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Common" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div id="divind" runat="server" align="left" style="overflow: auto; width: 760px;
                        height: 1180px; border-radius: 10px; border: 1px solid Gray;">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_popallow" runat="server" OnCheckedChanged="cb_popallow_CheckedChange"
                                        Style="margin-left: 20px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Allowances" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_popallow" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P10" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                width: 250px;">
                                                <asp:CheckBox ID="cb_popallowance" runat="server" Text="Select All" OnCheckedChanged="cb_popallowance_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_popallowance" runat="server" OnSelectedIndexChanged="cbl_popallowance_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_popallow"
                                                PopupControlID="P10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_addall" runat="server" Text="Add" Font-Bold="true" Font-Names="Book Antiqua"
                                        Visible="false" CssClass="textbox textbox1 btn2" OnClick="btnaddall_click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="allow_div" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: auto; margin-left: 75px;
                            width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lbl_header1" runat="server" Text="Allowances -" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                <asp:Label ID="lbl_h1" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbl_mode" runat="server" Text="Mode" Style="font-weight: bold; margin-left: 82px;
                                font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:DropDownList ID="ddl_mode" runat="server" OnSelectedIndexChanged="ddl_mode_indexchanged"
                                AutoPostBack="true" Style="background-color: #ffccff">
                                <asp:ListItem>Amount</asp:ListItem>
                                <asp:ListItem>Percent</asp:ListItem>
                                <asp:ListItem>Slab</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbl_value" runat="server" Text="Value" Style="font-weight: bold; margin-left: 30px;
                                font-family: book antiqua; font-size: medium;"></asp:Label>
                            <%--checkFloatValue(this);--%>
                            <asp:TextBox ID="txt_val" runat="server" onblur="checkamnt(this);" onkeyup="checkamnt(this);"
                                CssClass="textbox txtheight1" MaxLength="15" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterno" runat="server" FilterMode="ValidChars"
                                FilterType="Numbers,Custom" ValidChars="." TargetControlID="txt_val">
                            </asp:FilteredTextBoxExtender>
                            <table style="margin-left: 70px; margin-top: 20px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_lop" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Include LOP" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fbasic" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rball" Enabled="false"
                                            Text="From Basic" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fbgp" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rball" Enabled="false"
                                            Text="From Basic+GP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_special" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Is Special Allowances" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_agp" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rball" Enabled="false"
                                            Text="From Basic+AGP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_round" runat="server" Text="Round Type" Style="font-weight: bold;
                                            margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:DropDownList ID="ddl_round" runat="server" Style="background-color: #ffccff">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>>=5</asp:ListItem>
                                            <asp:ListItem><=5</asp:ListItem>
                                            <asp:ListItem>>=1</asp:ListItem>
                                            <asp:ListItem>=1</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="cb_fromallallow" runat="server" OnCheckedChanged="cb_fromallallow_CheckedChange"
                                            Style="margin-left: 0px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                            Enabled="false" AutoPostBack="true" Text="From Allow" />
                                    </td>
                                    <%--delsi0405--%>
                                    <td colspan="4">
                                        <asp:TextBox ID="txt_all_allowVal" runat="server" Enabled="false" Width="292px" Height="20px"
                                            CssClass="textbox1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Label ID="lbl_allowalert" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btn_allowsave" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btn_allowsave_Click" Text="Save" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_allowupdate" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_allowupdate_Click" Text="Update" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_allowdelete" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_allowdelete_Click" Text="Delete" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_allowexit" runat="server" OnClick="btn_allowexit_Click" Font-Bold="true"
                                    Font-Names="Book Antiqua" Text="Exit" CssClass="textbox textbox1 btn2" />
                            </center>
                            <br />
                        </div>
                        <div id="divgrdall" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 650px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grd_all" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowCreated="OnRowCreated" OnRowDataBound="grdall_rowbound"
                                        OnRowCommand="grdall_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Allowance Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_alltype" runat="server" Text='<%#Eval("alltype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_mode" runat="server" Text='<%#Eval("mode") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_val" runat="server" Text='<%#Eval("value") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncLop" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_lop" runat="server" Text='<%#Eval("inclop") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasic" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_frmbasic" runat="server" Text='<%#Eval("frmbasic") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_frmbasgp" runat="server" Text='<%#Eval("frmbasgp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsSplAllow" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_isspl" runat="server" Text='<%#Eval("isspl") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicAGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_frmbasagp" runat="server" Text='<%#Eval("frmbasagp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Round Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_roundtype" runat="server" Text='<%#Eval("roundval") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <%--delsi0405--%>
                                            <asp:TemplateField HeaderText="FromAllow" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_fromallallow" runat="server" Text='<%#Eval("FromAllow") %>'>
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
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_popdeduct" runat="server" OnCheckedChanged="cb_popdeduct_CheckedChange"
                                        Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Deductions" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_popdeduct" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P11" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                width: 250px;">
                                                <asp:CheckBox ID="cb_popdd" runat="server" Text="Select All" OnCheckedChanged="cb_popdd_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_popdd" runat="server" OnSelectedIndexChanged="cbl_popdd_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_popdeduct"
                                                PopupControlID="P11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnaddded" runat="server" Text="Add" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" CssClass="textbox textbox1 btn2" OnClick="btnaddded_click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="deduct_div" runat="server" visible="false" style="border: 2px solid indigo;
                            overflow: auto; border-radius: 10px; background-color: silver; height: auto;
                            margin-left: 75px; width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lbl_h2" runat="server" Text="Deductions -" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                <asp:Label ID="h2" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbl_dmode" runat="server" Text="Mode" Style="font-weight: bold; margin-left: 10px;
                                font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:DropDownList ID="ddl_dmode" runat="server" OnSelectedIndexChanged="ddl_dmode_indexchanged"
                                AutoPostBack="true" Style="background-color: #ffccff">
                                <asp:ListItem>Amount</asp:ListItem>
                                <asp:ListItem>Percent</asp:ListItem>
                                <asp:ListItem>Slab</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbl_dvalue" runat="server" Text="Value" Style="font-weight: bold;
                                margin-left: 30px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_dval" runat="server" onblur="chkamntded(this);" onkeyup="chkamntded(this);"
                                CssClass="textbox txtheight1" MaxLength="15" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filtertxtval" runat="server" FilterMode="ValidChars"
                                FilterType="Numbers,Custom" ValidChars="." TargetControlID="txt_dval">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="cb_fg" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Gross" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fbda" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic+DA" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_ilop" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Include LOP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="cb_fbgpda" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic+GP+DA" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fb" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fbdp" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic+DP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="cb_fp" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Petty" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_rt" runat="server" Text="Round Type" Style="font-weight: bold;
                                            margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:DropDownList ID="ddl_rt" runat="server" Style="background-color: #ffccff">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>>=5</asp:ListItem>
                                            <asp:ListItem><=5</asp:ListItem>
                                            <asp:ListItem>>=1</asp:ListItem>
                                            <asp:ListItem>=1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cb_fbarr" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic+Arrear" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_mcal" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Is Max Cal" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_mamt" runat="server" Text="Max Amt" Style="font-weight: bold;
                                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:TextBox ID="txt_mamt" runat="server" onblur="chkdoubleamnt(this);" onkeyup="chkdoubleamnt(this);"
                                            CssClass="textbox txtheight" MaxLength="15" Style="font-weight: bold; width: 120px;
                                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtermaxamnt" runat="server" FilterMode="ValidChars"
                                            FilterType="Numbers,Custom" ValidChars="." TargetControlID="txt_mamt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_damt" runat="server" Text="Ded Amt" Style="font-weight: bold;
                                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:TextBox ID="txt_damt" runat="server" onblur="return chkamntded(this);" onkeyup="return chkamntded(this);"
                                            CssClass="textbox txtheight" MaxLength="15" Style="font-weight: bold; width: 120px;
                                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterdamnt" runat="server" FilterMode="ValidChars"
                                            FilterType="Numbers,Custom" ValidChars="." TargetControlID="txt_damt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="cb_fbas" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From Basic+Arrear+SA" />
                                    </td>
                                    <td colspan="4">
                                        <asp:CheckBox ID="cb_fallow" runat="server" OnCheckedChanged="cb_fallow_CheckedChange"
                                            Style="margin-left: 0px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                            Enabled="false" AutoPostBack="true" Text="From Allow" />
                                        <asp:TextBox ID="txtcomded" runat="server" Enabled="false" Width="292px" Height="20px"
                                            CssClass="textbox1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rb_frmnet" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rbded" Enabled="false"
                                            Text="From NetAmount" />
                                    </td>
                                    <td>
                                        <%--poomalar 24.10.17--%>
                                        <asp:RadioButton ID="cb_professionaltax" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rbded"
                                            Enabled="true" Text="PT Gross Include LOP" />
                                        <%-- <asp:CheckBox ID="cb_professionaltax" runat="server" Text="PT Gross Include LOP" AutoPostBack="true" OnCheckedChanged="cb_professionaltax_CheckedChanged" />--%>
                                    </td>
                                    <td>
                                        <%--delsi 16.04.2018--%>
                                        <asp:RadioButton ID="radBtn_grosswithlop" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" Visible="false"
                                            Enabled="true" Text="From Selected Allow(lOP)" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Label ID="lbl_dedalert" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btn_deductsave" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btn_deductsave_Click" Text="Save" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_deductupdate" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_deductupdate_Click" Text="Update" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_deductdelete" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_deductdelete_Click" Text="Delete" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_deductexit" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btn_deductexit_Click" Text="Exit" CssClass="textbox textbox1 btn2" />
                                <asp:LinkButton ID="lnkNote" runat="server" Text="Note" OnClick="lnk_note_click"></asp:LinkButton>
                            </center>
                            <br />
                        </div>
                        <div id="divgrdded" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 703px;">
                            <div style="height: 200px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grid_ded" runat="server" Style="border-radius: 10px;" OnRowDataBound="grid_ded_rowbound"
                                        OnRowCommand="grid_ded_rowcommand" OnRowCreated="OnRowCreate_deduct" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Deduction Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_deducttype" runat="server" Text='<%#Eval("dedtype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedmode" runat="server" Text='<%#Eval("mode") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedval" runat="server" Text='<%#Eval("value") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Round Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_rounddedroundtype" runat="server" Text='<%#Eval("dedround") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Gross" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmgross" runat="server" Text='<%#Eval("frmgross") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicDA" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_frmbasicda" runat="server" Text='<%#Eval("frmbasicda") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncLop" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmlop" runat="server" Text='<%#Eval("inclop") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasic+GP+DA" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedgpda" runat="server" Text='<%#Eval("frmbasgpda") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmbas" runat="server" Text='<%#Eval("frmbas") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+DP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmbasdp" runat="server" Text='<%#Eval("frmbasdp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Petty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmpetty" runat="server" Text='<%#Eval("frmpetty") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+Arrear" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmbasarr" runat="server" Text='<%#Eval("frmbasarr") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is Max Cal" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedismaxcal" runat="server" Text='<%#Eval("ismaxcal") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Amt" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_maxamt" runat="server" Text='<%#Eval("maxamnt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ded Amt" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedamt" runat="server" Text='<%#Eval("dedamt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+Arrear+SA" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmbasarrsa" runat="server" Text='<%#Eval("frmbasarrsa") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Allow" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_dedfrmallow" runat="server" Text='<%#Eval("frmallow") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From NetAmount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_frmnetamnt" runat="server" Text='<%#Eval("frmnetamnt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gross LOP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_GrossLOP" runat="server" Text='<%#Eval("GrossLOP") %>'>
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
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_popltype" OnCheckedChanged="cb_popltype_CheckedChange" runat="server"
                                        Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Leave Type" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_popltype" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P12" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                width: 250px;">
                                                <asp:CheckBox ID="cb_poplt" runat="server" Text="Select All" OnCheckedChanged="cb_poplt_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_poplt" runat="server" OnSelectedIndexChanged="cbl_poplt_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_popltype"
                                                PopupControlID="P12" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnlevadd" runat="server" Text="Add" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" CssClass="textbox textbox1 btn2" OnClick="btnlevadd_click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="ltype_div" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: auto; margin-left: 75px;
                            width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lbl_h3" runat="server" Text="Leave Type -" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                                <asp:Label ID="h3" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbl_yl" runat="server" Text="Yearly Leaves" Style="font-weight: bold;
                                margin-left: 25px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_yl" runat="server" CssClass="textbox txtheight1" onkeyup="return chkleave()"
                                onblur="return chkleave()" OnTextChanged="txt_y1_txtchange"  AutoPostBack="true" MaxLength="5" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_yl"
                                FilterType="Custom,Numbers" ValidChars=" .">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="lbl_ml" runat="server" Text="Monthly Leaves" Style="font-weight: bold;
                                margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_ml" runat="server" CssClass="textbox txtheight1" onkeyup="return chkleave()"
                                onblur="return chkleave()" MaxLength="4" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_ml"
                                FilterType="Custom,Numbers" ValidChars=" .">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <table style="margin-left: 110px; margin-top: 10px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_sunday" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Checked="true" Text="Sunday Included" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_holiday" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="Holiday Included" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_mco" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="MonthlyCarryOver" 
                                            />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_yco" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="YearlyCarryOver" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <div id="div_GV1" runat="server" style="width: 445px; height: 210px; overflow: auto;">
                                    <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                                        GridLines="Both" OnRowDataBound="OnRowDataBound_gv1">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmonth" runat="server" Text='<%# Eval("Lblmonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Leave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlmaxleave" Width="70px" runat="server" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Date" HeaderStyle-BackColor="#0CA6CA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtfromdate" runat="server" Text='<%# Eval("txtfdate") %>' Font-Names="Book Antiqua" Font-Size="Small"
                                                        Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txtfromdate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To Date"  HeaderStyle-BackColor="#0CA6CA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txttodate" runat="server" Text='<%# Eval("txttdate") %>' Font-Names="Book Antiqua" Font-Size="Small"
                                                        Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" TargetControlID="txttodate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="lbl_ltypealert" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btn_ltypesave" runat="server" Text="Save" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btn_ltypesave_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_ltypeupdate" runat="server" Visible="false" Text="Update" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_ltypeupdate_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_ltypedelete" runat="server" Visible="false" Text="Delete" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_ltypedelete_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_ltypeexit" runat="server" Text="Exit" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btn_ltypeexit_Click" CssClass="textbox textbox1 btn2" />
                            </center>
                            <br />
                        </div>
                        <div id="divgrdlev" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 550px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdlev" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowCreated="OnRowCreated_Leave" OnRowDataBound="grdlev_rowbound"
                                        OnRowCommand="grdlev_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Leave Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_levtype" runat="server" Text='<%#Eval("levtype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Yearly Leave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yrlev" runat="server" Text='<%#Eval("yrlev") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monthly Leave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_monlev" runat="server" Text='<%#Eval("monlev") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncSunday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_incsunday" runat="server" Text='<%#Eval("incsunday") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncHoliday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_incholiday" runat="server" Text='<%#Eval("incholiday") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monthly C/O" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_moncarry" runat="server" Text='<%#Eval("moncarry") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Yearly C/O" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yrcarry" runat="server" Text='<%#Eval("yrcarry") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MonthlyMaxLeave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_MonthlyMaxLeave" runat="server" Text='<%#Eval("MonthlyMaxLeave") %>'>
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
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_common" OnCheckedChanged="chk_common_CheckedChanged" runat="server"
                                        Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"
                                        AutoPostBack="true" Text="Common" />
                                    <asp:Button ID="btnaddcom" runat="server" Text="Add" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnaddcom_click" CssClass="textbox btn2" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divcommon" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: 320px; margin-left: 75px;
                            width: 615px;">
                            <br />
                            <center>
                                <asp:Label ID="lblcom" runat="server" Text="Common" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                                <asp:Label ID="lblcomerr" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblgrdpay" runat="server" Text="Grade Pay" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtgrad" runat="server" CssClass="textbox textbox1 txtheight1" onkeyup="chkdoubleamnt(this);"
                                                MaxLength="15" Style="font-weight: bold; width: 120px; font-family: book antiqua;
                                                font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtertxtgrad" runat="server" TargetControlID="txtgrad"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbasiccom" runat="server" Text="Basic Pay" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbasiccom" runat="server" CssClass="textbox textbox1 txtheight1"
                                                onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold; width: 120px;
                                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtertxtbasiccom" runat="server" TargetControlID="txtbasiccom"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblpayband" runat="server" Text="Pay Band" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpayband" runat="server" CssClass="textbox textbox1 txtheight1"
                                                onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold; width: 120px;
                                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterpayband" runat="server" TargetControlID="txtpayband"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbismpfamnt" runat="server" Text="Is MPF" OnCheckedChanged="cbismpfamnt_change"
                                                AutoPostBack="true" Style="font-weight: bold; font-family: book antiqua; font-size: medium;">
                                            </asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbisautogp" runat="server" Text="Is AutoGP" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblismpf" runat="server" Text="MPF Amount" Visible="false" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtismpf" runat="server" CssClass="textbox textbox1 txtheight1"
                                                Visible="false" onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtermpfamnt" runat="server" TargetControlID="txtismpf"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblismpfper" runat="server" Text="MPF%" Visible="false" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtismpfper" runat="server" onkeyup="return percent()" onblur="return percent()"
                                                Visible="false" MaxLength="4" CssClass="textbox textbox1 txtheight1" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtismpfper"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="lblerrco" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <center>
                                <asp:Button ID="btnsavecom" runat="server" Text="Save" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnsavecom_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnupdatecom" runat="server" Text="Update" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnupdatecom_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btndelcom" runat="server" Text="Delete" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btndelcom_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnexitcom" runat="server" Text="Exit" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnexitcom_Click" CssClass="textbox textbox1 btn2" />
                            </center>
                        </div>
                        <div id="divgrdcom" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 400px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdcom" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowCreated="OnRowCreated_Common" OnRowDataBound="grdcom_rowbound"
                                        OnRowCommand="grdcom_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Grade Pay" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_grad" runat="server" Text='<%#Eval("gradepay") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Basic Pay" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_basicpay" runat="server" Text='<%#Eval("basicpay") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Band" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_payband" runat="server" Text='<%#Eval("payband") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_ismpf" runat="server" Text='<%#Eval("ismpf") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_ismpfper" runat="server" Text='<%#Eval("ismpfper") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_ismpfamnt" runat="server" Text='<%#Eval("ismpfamnt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is AutoGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_isautogp" runat="server" Text='<%#Eval("isautogp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </center>
                            </div>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btngrade" runat="server" Text="Set Grade Pay" Font-Bold="true" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1 btn2" Width="140px" OnClick="btngrade_click" />
                            <asp:Button ID="btngradeexit" runat="server" Text="Exit" Font-Bold="true" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1 btn2" OnClick="btngradeexit_click" />
                        </center>
                        <br />
                        <center>
                            <asp:Label ID="stdlblerr" runat="server" Visible="false"></asp:Label>
                        </center>
                    </div>
                    <div id="divoverall" runat="server" align="left" style="overflow: auto; width: 760px;
                        height: 1180px; border-radius: 10px; border: 1px solid Gray;">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbloverallow" runat="server" Visible="false" Style="margin-left: 20px;
                                        font-weight: bold; font-family: book antiqua; font-size: medium;" Text="Allowances"></asp:Label>
                                    <asp:Label ID="lbloverded" runat="server" Visible="false" Style="margin-left: 20px;
                                        font-weight: bold; font-family: book antiqua; font-size: medium;" Text="Deduction"></asp:Label>
                                    <asp:Label ID="lbloverleav" runat="server" Visible="false" Style="margin-left: 20px;
                                        font-weight: bold; font-family: book antiqua; font-size: medium;" Text="Leave Type"></asp:Label>
                                    <asp:Label ID="lblovercom" runat="server" Visible="false" Style="margin-left: 20px;
                                        font-weight: bold; font-family: book antiqua; font-size: medium;" Text="Common"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddloverallallow" runat="server" OnSelectedIndexChanged="ddloverallallow_change"
                                        AutoPostBack="true" Visible="false" CssClass="textbox1 ddlheight5">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddloverallded" runat="server" OnSelectedIndexChanged="ddloverallded_change"
                                        AutoPostBack="true" Visible="false" CssClass="textbox1 ddlheight5">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddloverlev" runat="server" OnSelectedIndexChanged="ddloverlev_change"
                                        AutoPostBack="true" Visible="false" CssClass="textbox1 ddlheight5">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="allowover_div" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: auto; margin-left: 75px;
                            width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lblalllabel" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbloverallmode" runat="server" Text="Mode" Style="font-weight: bold;
                                margin-left: 82px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:DropDownList ID="ddloverallmode" runat="server" OnSelectedIndexChanged="ddloverallmode_indexchanged"
                                AutoPostBack="true" Style="background-color: #ffccff">
                                <asp:ListItem>Amount</asp:ListItem>
                                <asp:ListItem>Percent</asp:ListItem>
                                <asp:ListItem>Slab</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbloverallval" runat="server" Text="Value" Style="font-weight: bold;
                                margin-left: 30px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txtoverallval" runat="server" onblur="checkoverallamnt(this);" onkeyup="checkoverallamnt(this);"
                                CssClass="textbox txtheight1" MaxLength="15" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtoverallval">
                            </asp:FilteredTextBoxExtender>
                            <table style="margin-left: 70px; margin-top: 20px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbinclopoverall" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Include LOP" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbfrmbasoverall" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rboverall" Enabled="false"
                                            Text="From Basic" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbfrmbasgpoverall" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rboverall" Enabled="false"
                                            Text="From Basic+GP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbissploverall" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Is Special Allowances" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbfrmbasagp" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rboverall" Enabled="false"
                                            Text="From Basic+AGP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblroundoverall" runat="server" Text="Round Type" Style="font-weight: bold;
                                            margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:DropDownList ID="ddlroundoverall" runat="server" Style="background-color: #ffccff">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>>=5</asp:ListItem>
                                            <asp:ListItem><=5</asp:ListItem>
                                            <asp:ListItem>>=1</asp:ListItem>
                                            <asp:ListItem>=1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Label ID="lblerroverall" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btn_overallsave" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_overallsave_Click" Text="Save" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_overallupdate" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_overallupdate_Click" Text="Update" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_overalldelete" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btn_overalldelete_click" Text="Delete" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btn_overallexit" runat="server" OnClick="btn_overallexit_Click" Font-Bold="true"
                                    Font-Names="Book Antiqua" Text="Exit" CssClass="textbox textbox1 btn2" />
                            </center>
                            <br />
                        </div>
                        <div id="divoverallgrd" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 650px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdoverall" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowDataBound="grdoverall_rowbound" OnRowCommand="grdoverall_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Allowance Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overalltype" runat="server" Text='<%#Eval("overalltype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallmode" runat="server" Text='<%#Eval("overallmode") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallval" runat="server" Text='<%#Eval("overallvalue") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncLop" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overalllop" runat="server" Text='<%#Eval("overallinclop") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasic" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallfrmbasic" runat="server" Text='<%#Eval("overallfrmbasic") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallfrmbasgp" runat="server" Text='<%#Eval("overallfrmbasgp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsSplAllow" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallisspl" runat="server" Text='<%#Eval("overallisspl") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicAGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallfrmbasagp" runat="server" Text='<%#Eval("overallfrmbasagp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Round Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overallroundtype" runat="server" Text='<%#Eval("overallroundval") %>'>
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
                        <div id="dedover_div" runat="server" visible="false" style="border: 2px solid indigo;
                            overflow: auto; border-radius: 10px; background-color: silver; height: auto;
                            margin-left: 75px; width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lbldedalllbl" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbloverdedmode" runat="server" Text="Mode" Style="font-weight: bold;
                                margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:DropDownList ID="ddloverdedmode" runat="server" OnSelectedIndexChanged="ddloverdedmode_indexchanged"
                                AutoPostBack="true" Style="background-color: #ffccff">
                                <asp:ListItem>Amount</asp:ListItem>
                                <asp:ListItem>Percent</asp:ListItem>
                                <asp:ListItem>Slab</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbloverdedval" runat="server" Text="Value" Style="font-weight: bold;
                                margin-left: 30px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txtoverdedval" runat="server" onblur="chkamntoverded(this);" onkeyup="chkamntoverded(this);"
                                CssClass="textbox txtheight1" MaxLength="15" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtoverdedval">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmgross" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Gross" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbasda" runat="server" Style="margin-left: 0px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Basic+DA" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbinclopoverded" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Include LOP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbasgpda" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Basic+GP+DA" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbas" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rboverded" Enabled="false"
                                            Text="From Basic" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbasdp" runat="server" Style="margin-left: 0px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Basic+DP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmpetty" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Petty" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbloverdedround" runat="server" Text="Round Type" Style="font-weight: bold;
                                            margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:DropDownList ID="ddloverdedround" runat="server" Style="background-color: #ffccff">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>>=5</asp:ListItem>
                                            <asp:ListItem><=5</asp:ListItem>
                                            <asp:ListItem>>=1</asp:ListItem>
                                            <asp:ListItem>=1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbasarr" runat="server" Style="margin-left: 0px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Basic+Arrear" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbmaxcaloverded" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Enabled="false" Text="Is Max Cal" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblmaxamntoverded" runat="server" Text="Max Amt" Style="font-weight: bold;
                                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:TextBox ID="txtoverdedmaxamnt" runat="server" onblur="chkdoubleamnt(this);"
                                            onkeyup="chkdoubleamnt(this);" CssClass="textbox txtheight" MaxLength="15" Style="font-weight: bold;
                                            width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                            FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtoverdedmaxamnt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldedamntoverded" runat="server" Text="Ded Amt" Style="font-weight: bold;
                                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:TextBox ID="txtoverdeddedamnt" runat="server" onblur="return chkamntoverded(this);"
                                            onkeyup="return chkamntoverded(this);" CssClass="textbox txtheight" MaxLength="15"
                                            Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                            FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtoverdeddedamnt">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmbasarrsa" runat="server" Style="margin-left: 5px;
                                            font-weight: bold; font-family: book antiqua; font-size: medium;" GroupName="rboverded"
                                            Enabled="false" Text="From Basic+Arrear+SA" />
                                    </td>
                                    <td colspan="4">
                                        <asp:CheckBox ID="cbfallowoverded" runat="server" OnCheckedChanged="cbfallowoverded_CheckedChange"
                                            Style="margin-left: 0px; font-weight: bold; font-family: book antiqua; font-size: medium;"
                                            Enabled="false" AutoPostBack="true" Text="From Allow" />
                                        <asp:TextBox ID="txtoverdedall" runat="server" Enabled="false" Width="292px" Height="20px"
                                            CssClass="textbox1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdboverdedfrmnet" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" GroupName="rboverded" Enabled="false"
                                            Text="From NetAmount" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Label ID="lbloverdederr" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btnoverdedsave" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnoverdedsave_Click" Text="Save" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnoverdedupdate" runat="server" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btnoverdedupdate_Click" Text="Update" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnoverdeddel" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnoverdeddel_Click" Text="Delete" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnoverdedexit" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnoverdedexit_Click" Text="Exit" CssClass="textbox textbox1 btn2" />
                            </center>
                            <br />
                        </div>
                        <div id="divoverdedgrd" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 703px;">
                            <div style="height: 200px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdoverded" runat="server" Style="border-radius: 10px;" OnRowDataBound="grdoverded_rowbound"
                                        OnRowCommand="grdoverded_rowcommand" AutoGenerateColumns="false" Visible="false"
                                        GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Deduction Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdeducttype" runat="server" Text='<%#Eval("overdedtype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedmode" runat="server" Text='<%#Eval("overdedmode") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedval" runat="server" Text='<%#Eval("overdedvalue") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Round Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overrounddedroundtype" runat="server" Text='<%#Eval("overdedround") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Gross" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmgross" runat="server" Text='<%#Eval("overdedfrmgross") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasicDA" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmbasicda" runat="server" Text='<%#Eval("overdedfrmbasicda") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncLop" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmlop" runat="server" Text='<%#Eval("overdedinclop") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FromBasic+GP+DA" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedgpda" runat="server" Text='<%#Eval("overdedfrmbasgpda") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdeddedfrmbas" runat="server" Text='<%#Eval("overdedfrmbas") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+DP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmbasdp" runat="server" Text='<%#Eval("overdedfrmbasdp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Petty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmpetty" runat="server" Text='<%#Eval("overdedfrmpetty") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+Arrear" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmbasarr" runat="server" Text='<%#Eval("overdedfrmbasarr") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is Max Cal" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedismaxcal" runat="server" Text='<%#Eval("overdedismaxcal") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Amt" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedmaxamt" runat="server" Text='<%#Eval("overdedmaxamnt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ded Amt" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedamt" runat="server" Text='<%#Eval("overdedamt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Basic+Arrear+SA" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmbasarrsa" runat="server" Text='<%#Eval("overdedfrmbasarrsa") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Allow" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmallow" runat="server" Text='<%#Eval("overdedfrmallow") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From NetAmount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overdedfrmnetamnt" runat="server" Text='<%#Eval("overdedfrmnetamnt") %>'>
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
                        <div id="ltype_overlevdiv" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: auto; margin-left: 75px;
                            width: auto;">
                            <br />
                            <center>
                                <asp:Label ID="lbloverlev" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="lbloveryrlev" runat="server" Text="Yearly Leaves" Style="font-weight: bold;
                                margin-left: 25px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txtoveryrlev" runat="server" CssClass="textbox txtheight1" onkeyup="return chkoverleave()"
                                onblur="return chkoverleave()" MaxLength="3" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtoveryrlev"
                                FilterType="Numbers" ValidChars="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="lblovermonlev" runat="server" Text="Monthly Leaves" Style="font-weight: bold;
                                margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txtovermonlev" runat="server" CssClass="textbox txtheight1" onkeyup="return chkoverleave()"
                                onblur="return chkoverleave()" MaxLength="2" Style="font-weight: bold; width: 120px;
                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtovermonlev"
                                FilterType="Numbers" ValidChars="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <table style="margin-left: 110px; margin-top: 10px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cboversuninc" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Checked="true" Text="Sunday Included" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cboverholinc" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="Holiday Included" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbovermonco" runat="server" Style="margin-left: 5px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="MonthlyCarryOver" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cboveryrco" runat="server" Style="margin-left: 0px; font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" Text="YearlyCarryOver" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Label ID="lbloverleverr" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btnltypeoversave" runat="server" Text="Save" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnltypeoversave_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnltypeoverupdate" runat="server" Visible="false" Text="Update"
                                    Font-Bold="true" Font-Names="Book Antiqua" OnClick="btnltypeoverupdate_Click"
                                    CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnltypeoverdel" runat="server" Visible="false" Text="Delete" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btnltypeoverdel_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnltypeoverexit" runat="server" Text="Exit" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnltypeoverexit_Click" CssClass="textbox textbox1 btn2" />
                            </center>
                            <br />
                        </div>
                        <div id="divoveralllev" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 550px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdoveralllev" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowDataBound="grdoveralllev_rowbound" OnRowCommand="grdoveralllev_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Leave Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overlevtype" runat="server" Text='<%#Eval("overlevtype") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Yearly Leave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overyrlev" runat="server" Text='<%#Eval("overyrlev") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monthly Leave" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overmonlev" runat="server" Text='<%#Eval("overmonlev") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncSunday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overincsunday" runat="server" Text='<%#Eval("overincsunday") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncHoliday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overincholiday" runat="server" Text='<%#Eval("overincholiday") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monthly C/O" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overmoncarry" runat="server" Text='<%#Eval("overmoncarry") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Yearly C/O" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overyrcarry" runat="server" Text='<%#Eval("overyrcarry") %>'>
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
                        <div id="divovercom" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; background-color: silver; height: 320px; margin-left: 75px;
                            width: 615px;">
                            <br />
                            <center>
                                <asp:Label ID="lbloverallcom" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua;
                                    font-size: large; color: green;"></asp:Label>
                            </center>
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblovergrade" runat="server" Text="Grade Pay" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtovergrade" runat="server" CssClass="textbox textbox1 txtheight1"
                                                onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold; width: 120px;
                                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtovergrade"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbloverbasic" runat="server" Text="Basic Pay" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtoverbasic" runat="server" CssClass="textbox textbox1 txtheight1"
                                                onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold; width: 120px;
                                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtoverbasic"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbloverpayband" runat="server" Text="Pay Band" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtoverpayband" runat="server" CssClass="textbox textbox1 txtheight1"
                                                onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold; width: 120px;
                                                font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtoverpayband"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbismpfover" runat="server" Text="Is MPF" OnCheckedChanged="cbismpfover_change"
                                                AutoPostBack="true" Style="font-weight: bold; font-family: book antiqua; font-size: medium;">
                                            </asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbisautogpover" runat="server" Text="Is AutoGP" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblismpfamntover" runat="server" Text="MPF Amount" Visible="false"
                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtismpfamntover" runat="server" CssClass="textbox textbox1 txtheight1"
                                                Visible="false" onkeyup="chkdoubleamnt(this);" MaxLength="15" Style="font-weight: bold;
                                                width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtismpfamntover"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblismpfperover" runat="server" Text="MPF%" Visible="false" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtismpfperover" runat="server" onkeyup="return overallpercent()"
                                                onblur="return overallpercent()" Visible="false" MaxLength="4" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtismpfperover"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="lblovercomerr" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                            </center>
                            <center>
                                <asp:Button ID="btnovercomsave" runat="server" Visible="false" Text="Save" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btnovercomsave_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnovercomupdate" runat="server" Visible="false" Text="Update" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btnovercomupdate_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnovercomdel" runat="server" Visible="false" Text="Delete" Font-Bold="true"
                                    Font-Names="Book Antiqua" OnClick="btnovercomdel_Click" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnovercomexit" runat="server" Text="Exit" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnovercomexit_Click" CssClass="textbox textbox1 btn2" />
                            </center>
                        </div>
                        <div id="divovergrdcom" runat="server" visible="false" style="border: 2px solid indigo;
                            border-radius: 10px; height: 200px; margin-left: 27px; width: 400px;">
                            <div style="height: 150px; overflow: auto;">
                                <center>
                                    <asp:GridView ID="grdovercom" runat="server" Style="border-radius: 10px;" AutoGenerateColumns="false"
                                        Visible="false" GridLines="Both" OnRowDataBound="grdovercom_rowbound" OnRowCommand="grdovercom_rowcommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Grade Pay" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overgrad" runat="server" Text='<%#Eval("overgradepay") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Basic Pay" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overbasicpay" runat="server" Text='<%#Eval("overbasicpay") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Band" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overpayband" runat="server" Text='<%#Eval("overpayband") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overismpf" runat="server" Text='<%#Eval("overismpf") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overismpfper" runat="server" Text='<%#Eval("overismpfper") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is MPF" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overismpfamnt" runat="server" Text='<%#Eval("overismpfamnt") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is AutoGP" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_overisautogp" runat="server" Text='<%#Eval("overisautogp") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </center>
                            </div>
                        </div>
                        <br />
                        <center>
                            <asp:Label ID="com_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btnoversetgrade" runat="server" Text="Set Grade Pay" OnClick="btnoversetgrade_click"
                                CssClass="textbox textbox1 btn2" Width="140px" />
                            <asp:Button ID="btnoversetgrade_exit" runat="server" Text="Exit" OnClick="btnoversetgrade_exit_click"
                                CssClass="textbox textbox1 btn2" />
                        </center>
                    </div>
                    <center>
                        <div id="divallhead" runat="server" class="popupstyle popupheight1" visible="false"
                            style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                            <asp:ImageButton ID="imgallhead" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 89px; margin-left: 252px;"
                                OnClick="imgallhead_Click" />
                            <center>
                                <div id="divinnerhead" runat="server" class="table" style="background-color: White;
                                    height: 540px; width: 550px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    margin-top: 90px; border-radius: 10px;">
                                    <center>
                                        <br />
                                        <asp:Label ID="lblheadset" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Large" ForeColor="Green"></asp:Label>
                                        <br />
                                        <fieldset style="border-radius: 10px; width: 459px;">
                                            <legend style="font-size: larger; font-weight: bold">From Allowance</legend>
                                            <table class="table" style="float: left;">
                                                <tr>
                                                    <td>
                                                        Basic & Grade Pay
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_selbasgrad" runat="server" SelectionMode="Multiple" Height="100px"
                                                            Width="200px">
                                                            <asp:ListItem Text="Basic" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Grade Pay" Value="1"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Allowances
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_allowhdr" runat="server" SelectionMode="Multiple" Height="200px"
                                                            Width="200px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table" style="float: left;">
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
                                            <table class="table" style="float: left;">
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_selallow" runat="server" SelectionMode="Multiple" Height="350px"
                                                            Width="200px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <br />
                                        <asp:Label ID="lblheaderr" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Button ID="btnokall" runat="server" CssClass="textbox textbox1 btn2" Text="OK"
                                            OnClick="btnokall_click" />
                                        <asp:Button ID="btnexitallow" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                            OnClick="btnexitallow_click" />
                                    </center>
                                </div>
                            </center>
                        </div>
                        <%--delsi--%>
                        <div id="divallHeadallow" runat="server" class="popupstyle popupheight1" visible="false"
                            style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                            <asp:ImageButton ID="imgallHeadallow" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 89px; margin-left: 252px;"
                                OnClick="imgallheadallow_Click" />
                            <center>
                                <div id="div3" runat="server" class="table" style="background-color: White; height: 540px;
                                    width: 550px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
                                    border-radius: 10px;">
                                    <center>
                                        <br />
                                        <asp:Label ID="lblheadsetallow" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Large" ForeColor="Green"></asp:Label>
                                        <br />
                                        <fieldset style="border-radius: 10px; width: 459px;">
                                            <legend style="font-size: larger; font-weight: bold">From Allowance</legend>
                                            <table class="table" style="float: left;">
                                                <tr>
                                                    <td>
                                                        Basic & Grade Pay
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_selbasgrads" runat="server" SelectionMode="Multiple" Height="100px"
                                                            Width="200px">
                                                            <asp:ListItem Text="Basic" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Grade Pay" Value="1"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Allowances
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_allowhdrs" runat="server" SelectionMode="Multiple" Height="200px"
                                                            Width="200px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table" style="float: left;">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnMvOneRts" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnMvOneRt_Click_allow" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnMvTwoRts" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnMvTwoRt_Click_allow" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnMvOneLts" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnMvOneLt_Click_allow" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnMvTwoLts" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnMvTwoLt_Click_allow" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table" style="float: left;">
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lb_selallows" runat="server" SelectionMode="Multiple" Height="350px"
                                                            Width="200px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <br />
                                        <asp:Label ID="lblheaderrs" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Button ID="btnokall_allow" runat="server" CssClass="textbox textbox1 btn2" Text="OK"
                                            OnClick="btnokall_click_allow" />
                                        <asp:Button ID="btnexitallow_s" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                            OnClick="btnexitallow_click_allow" />
                                    </center>
                                </div>
                            </center>
                        </div>
                        <%--delsi--%>
                        <div id="alertpopwindow" runat="server" class="popupstyle popupheight1" visible="false"
                            style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                            Text="Ok" runat="server" OnClick="btnerrclose_Click" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="DivNote" runat="server" class="popupstyle popupheight1" visible="false"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <div id="suvid" runat="server" style="background-color: White; margin-left: 85px;
                    width: 660px; height: 450px;" align="center">
                    <asp:ImageButton ID="img_note" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -4px; margin-left: 654px;"
                        OnClick="img_note_Click" />
                    <asp:Label ID="noteid" runat="server" Text="">
                    
                    </asp:Label>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
