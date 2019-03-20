<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="FinancestudPaidDet.aspx.cs"
    Inherits="FinancestudPaidDet" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID%>').click(function () {
                var excelName = $('#<%=txtexcelname.ClientID%>').val();
                if (excelName == null || excelName == "") {
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').hide();
                }
            });

            $('#<%=txtexcelname.ClientID %>').keypress(function () {
                $('#<%=lblvalidation1.ClientID %>').hide();
            });

            $('#<%=btnMemPopup.ClientID %>').click(function () {
                var chkBoxList = document.getElementById('<%=cblmem.ClientID %>');
                var selectedCount = CheckBoxListSelectDept(chkBoxList);
                if (selectedCount != 1) {
                    alert("Please select any one Staff/Vendor/Other type!");
                    return false;
                }
            });
        });
        function CheckBoxListSelectDept(chkBoxList) {
            var totCount = 0;
            //            var chkBoxList = document.getElementById('<%=cblmem.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked)
                    totCount++;
            }
            return totCount;
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
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;

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
                document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                document.getElementById('<%=txt_todate.ClientID%>').value = today;
                alert("To date should be greater than from date ");
                return false;
            }
        }
        function columnOrderCbl() {
            var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
            txtval.value = "";
            var getval = "";
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            var tagnamestr = cblall.getElementsByTagName("label");
            if (cball.checked == true) {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = true;
                    if (getval == "")
                        getval = tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                    else
                        getval += ", " + tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                }
            }
            else {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = false;
                }
                getval = "";
                oldval = "";
            }
            if (getval != "") {
                txtval.value = getval.toString();
            }
        }

        function columnOrderCb() {
            var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
            var oldval = txtval.value.toString();
            txtval.value = "";
            var newval = "";
            var getval = "";
            var count = 0;
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            var tagnamestr = cblall.getElementsByTagName("label");
            for (var i = 0; i < tagname.length; i++) {
                if (tagname[i].checked == true) {
                    count += 1;
                    getval = tagnamestr[i].innerHTML; //current checked val
                    if (oldval != null && oldval != "") {
                        var result = oldval.includes(getval);
                        if (!result) {
                            oldval += "," + getval;
                        }
                    }
                    else {
                        oldval = getval;
                    }

                }
                else {
                    //                    if (oldval != null && oldval != "") {
                    //                        var result = oldval.includes(getval);
                    //                        if (result) {
                    //                            oldval = oldval.replace(getval, " ");
                    //                        }
                    //                    }
                }
            }
            if (tagname.length == count) {
                cball.checked = true;
            }
            else {
                cball.checked = false;
            }
            if (oldval != "") {
                txtval.value = oldval.toString();
            }
        }
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=GrdStaff.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_GrdStaff_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
       
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnAdd.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').show();
                $('#<%=txtdesc.ClientID %>').val('');
                return false;
            });
            $('#<%=btnDel.ClientID %>').click(function () {
                var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                if (rptText.trim() != null && rptText != "Select") {
                    var msg = confirm("Are you sure you want to delete this report type?");
                    if (msg)
                        return true;
                    else
                        return false;
                }
                else {
                    alert("Please select any one report type!");
                    return false;
                }
            });

            $('#<%=btnexittype.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').hide();
                return false;
            });

            $('#<%=btnaddtype.ClientID %>').click(function () {
                var txtval = $('#<%=txtdesc.ClientID %>').val();
                if (txtval == null || txtval == "") {
                    alert("Please enter the report type!");
                    return false;
                }
            });

            $('#<%=btnclear.ClientID %>').click(function () {
                $('#<%=txtcolorder.ClientID %>').val('');
                $("[id*=cblcolumnorder]").removeAttr('checked');
                return false;
            });

            $('#<%=imgcolumn.ClientID %>').click(function () {
                $('#<%=divcolorder.ClientID %>').hide();
                return false;
            });
            $('#<%=btngo.ClientID %>').click(function () {
                var rptText = $('#<%=ddlMainreport.ClientID %>').find('option:selected').text();
                if (rptText.trim() == null || rptText == "Select") {
                    alert("Please select any one report type!");
                    return false;
                }
            });

            $('#<%=btncolorderOK.ClientID %>').click(function () {
                var textval = $('#<%=txtcolorder.ClientID %>').val();
                if (textval != "" && textval != null) {
                    var credit = textval.includes("Credit");
                    var debit = textval.includes("Debit");
                    if (!credit && !debit) {
                        alert("Please Select Crdeit or Debit Type!");
                        return false;
                    }
                }
                else {
                    alert("Please Select column order Type!");
                    return false;
                }
            });
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Institutionwise
                    Paid Report</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 1138px; height: auto">
                        <table class="maintablestyle" border="0">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:RadioButtonList ID="rblMemType" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblMemType_Selected">
                                                <asp:ListItem Text="Student" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Others"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdmemtype" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlmem" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                height: 120px;">
                                                <asp:CheckBox ID="cbmem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbmem_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblmem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblmem_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtmem"
                                                PopupControlID="pnlmem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdMemPopup" runat="server" visible="false">
                                    <asp:Button ID="btnMemPopup" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                        OnClick="btnMemPopup_Click" />
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbldisp" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="White"></asp:Label>
                                    <asp:Label ID="lblval" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                                height: 120px;">
                                                <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbclg_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                                PopupControlID="pnlclg" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 300px; height: 180px;">
                                                <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                PopupControlID="pnl_studhed" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ledger" runat="server" Text="Ledger" Style="width: 50px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 300px; height: 180px;">
                                                <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_studled"
                                                PopupControlID="pnl_studled" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    PayMode
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upd_paid" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 126px; height: 160px;">
                                                <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                                PopupControlID="pnl_paid" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbluser" Text="User/Counter" Width="85px"></asp:Label>
                                </td>
                                <td id="td1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtuser" Style="height: 20px; width: 118px;" CssClass="Dropdown_Txt_Box"
                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="178px">
                                                <asp:CheckBox ID="cbuser" runat="server" Text="Select All" OnCheckedChanged="cbuser_changed"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cbluser" runat="server" OnSelectedIndexChanged="cbluser_selected"
                                                    AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtuser"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <div id="divdatewise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblmode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblmode_Selected"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Header" Value="0" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="Ledger" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>
                                    <%-- </td>
                        <td>--%>
                                    <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                        Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdlblfnl" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                </td>
                                <td id="tdfnl" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfyear" Style="height: 20px; width: 141px;" CssClass="Dropdown_Txt_Box"
                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                                <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                    AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                                PopupControlID="Pfyear" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2" id="tdlblStudCat" runat="server" visible="false">
                                    <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="width: 200px;" />
                                    <%--AutoPostBack="true" OnCheckedChanged="checkdicon_Changed"--%>
                                </td>
                                <td id="tdvalStudCat" runat="server" visible="false" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtinclude" Enabled="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Style="height: 20px; width: 150px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="200px" Style="height: auto;">
                                                <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                PopupControlID="pnlinclude" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdJournal" runat="server" visible="false">
                                    <asp:CheckBox ID="cbJournal" runat="server" Text="Journal" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset style="height: 23px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbAcdYear" runat="server" Text="" />
                                                    <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="102px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblTypeNew" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Academic Year" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Odd"></asp:ListItem>
                                                        <asp:ListItem Text="Even"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td id="tdOthers" runat="server" visible="false">
                                    <asp:CheckBox ID="cbIncOthers" runat="server" Text="Include Other" Checked="true" />
                                </td>
                                <td colspan="4" style="text-align: right">
                                    <asp:UpdatePanel ID="UpGo" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <div id="divlabl" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcash" runat="server" Text="Cash" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightCoral"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblchq" runat="server" Text="Cheque" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGray"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldd" runat="server" Text="DD" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" BackColor="Orange"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblchal" runat="server" Text="Challan" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGreen"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblonline" runat="server" Text="Online" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGoldenrodYellow"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcard" runat="server" Text="Card" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="white"></asp:Label>
                                    </td>
                                    <%-- Added By saranya on 13/2/2018--%>
                                    <td>
                                        <asp:Label ID="lblNeft" runat="server" Text="Neft" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="Aqua"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                    Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Print Setting" OnClick="btnprintmaster_Click" Height="32px"
                                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="100px" />
                                <%--added by deepali 02.11.2017--%>
                                <asp:Button ID="btn_print" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Print" OnClick="btn_print_Click" Height="32px" Style="margin-top: 10px;"
                                    CssClass="textbox textbox1" Width="60px" />
                                <%--------------------------------------------%>
                                 <NEW:NEWPrintMater runat="server" ID="Printcontrolhed" Visible="false" />
                            </div>
                        </center>
                        <br />
                        <asp:GridView ID="grdInstWisePaidReport" Width="900px" runat="server" ShowFooter="false"
                            AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false"
                            OnRowDataBound="grdInstWisePaidReport_RowDataBound">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                    <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                    <asp:PostBackTrigger ControlID="btn_print" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </div>
    <%--column order--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" onclick="close_click" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 304px;" />
                    <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 322px;
                            width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                            border-radius: 10px;">
                            <center>
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                                font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblrptype" Text="Report Type" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="textbox textbox1 btn1" OnClick="btnAdd_OnClick" /><%--OnClick="btnAdd_OnClick"--%>
                                                        </td>
                                                         <script type="text/javascript" language="javascript">
                                                             Sys.Application.add_load(click);
            </script>
                                                        <td>
                                                            <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                                CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDel" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnDel_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 600px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="600px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <center>
                                                <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                                <%--   </center>
                                </td>
                                <td>
                                    <center>--%>
                                                <asp:Button ID="btnclear" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    Text="Clear" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                    <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
                </div>
            </ContentTemplate>
             <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnAdd" />
                                                </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--report type name enter text box--%>
    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>
            <div id="divaddtype" runat="server" visible="false" style="height: 100%; z-index: 10000; width: 100%;
                background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                ">
                <center>
                    <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                        height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddtype_Click" />
                                    <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btnexittype_Click" /><%----%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </ContentTemplate>
        
    </asp:UpdatePanel>
    <%--Staff Lookup --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                        OnClick="btn_exitstaff_Click" />
                    <br />
                    <div style="background-color: White; height: 550px; width: 650px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span id="spnHdName" runat="server" class="fontstyleheader" style="color: Green;">
                                </span>
                            </div>
                        </center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <span class="challanLabel">
                                        <p>
                                            Search By</p>
                                    </span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpStaffGo" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_go2Staff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                                OnClick="btn_go2Staff_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Label ID="lbl_errormsgstaff" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <br />
                        <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                            <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                                onchange="return SelLedgers();" />
                        </span>
                        <div id="divTreeView" visible="false" runat="server" align="left" style="overflow: auto;
                            width: 520px; height: 350px; border-radius: 10px; border: 1px solid Gray;">
                            <asp:GridView ID="GrdStaff" Width="500px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="GrdStaff_RowDataBound">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="selectchk" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <center>
                            <div>
                                <asp:Button ID="btn_staffOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                    OnClick="btn_staffOK_Click" />
                                <asp:Button ID="btn_exitstaff" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                    OnClick="btn_exitstaff_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for UpGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpStaffGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpStaffGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
