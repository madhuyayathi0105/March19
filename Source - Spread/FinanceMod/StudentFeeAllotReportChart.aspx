<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StudentFeeAllotReportChart.aspx.cs" Inherits="StudentFeeAllotReportChart" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style>
        body
        {
            font-family:Book Antiqua;
            font-size:14px;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function checkDate() {
                var datewise = document.getElementById('<%=chk_datewise.ClientID%>');
                if (datewise.checked == true) {
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
                        //// document.getElementById('<%=txt_todate.ClientID %>').value = currentDate;
                        return false;
                    }
                }
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Student / Staff Fee Allotment ,
                        Paid, Concession and Balance Report Chart</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <div>
                        <table style="float: left;">
                            <tr>
                                <td>
                                    <center>
                                        <div>
                                            <table class="maintablestyle" style="float: left;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                            OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                            CssClass="textbox  ddlheight" Style="width: 108px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Batch
                                                    </td>
                                                    <td id="tdcblbatch" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UP_batch" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                    height: 200px;">
                                                                    <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                    PopupControlID="panel_batch" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdddlbatch" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_OnSelectedIndexChanged"
                                                            CssClass="textbox  ddlheight" Style="width: 80px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                    </td>
                                                    <td id="tdcbldegree" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UP_degree" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                    height: 200px;">
                                                                    <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
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
                                                    <td id="tdddldegree" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged"
                                                            CssClass="textbox  ddlheight" Style="width: 103px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                    </td>
                                                    <td id="tdcbldept" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="Up_dept" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                    height: 300px;">
                                                                    <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                                    PopupControlID="panel_dept" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdddldept" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddldept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldept_OnSelectedIndexChanged"
                                                            CssClass="textbox  ddlheight" Style="width: 140px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 124px;
                                                                    height: 172px;">
                                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                                                    PopupControlID="panel_sem" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        Section
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Updp_sect" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_sect" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                                    height: 100px;">
                                                                    <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sect"
                                                                    PopupControlID="panel_sect" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        Payment Mode
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upd_paid" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                    Style="width: 126px; height: 120px;">
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
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                    Style="width: 126px; height: 120px;">
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
                                                        <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                    Style="width: 126px; height: 120px;">
                                                                    <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                                    PopupControlID="pnl_studled" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        Date Wise
                                                        <%-- </td>
                                                <td>--%>
                                                        <asp:CheckBox ID="chk_datewise" runat="server" OnCheckedChanged="chk_datewise_OnCheckedChanged"
                                                            AutoPostBack="true" />
                                                    </td>
                                                    <td colspan="2">
                                                        <div id="divdatewise" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="margin-top: 10px;
                                                                            margin-left: -140px;"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_fromdate" runat="server" Enabled="false" Style="height: 20px;
                                                                            width: 79px; margin-left: -104px;" OnTextChanged="txt_fromdate_Textchanged"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 14px;"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_todate" runat="server" Enabled="false" Style="height: 20px;
                                                                            width: 95px; margin-left: 6px;" OnTextChanged="txt_todate_Textchanged"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkinclude" runat="server" Text=" Include Discontinue" AutoPostBack="true"
                                                            OnCheckedChanged="chkinclude_OnCheckedChanged" />
                                                    </td>
                                                    <td colspan="3">
                                                        <%--<asp:CheckBox ID="chkinclude" runat="server" Text=" Include Discontinue" Visible="false" />--%>
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtinclude" Style="height: 20px; width: 115px;" CssClass="Dropdown_Txt_Box"
                                                                    runat="server" Enabled="false" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel" Width="147px">
                                                                    <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" OnCheckedChanged="cbinclude_OnCheckedChanged"
                                                                        AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="cblinclude" runat="server" OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                                    PopupControlID="pnlinclude" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <%-- <td colspan="6">
                                                    <div id="divstudwise" runat="server" visible="false">
                                                        <fieldset style="width: 195px; height: 16px; margin-top: 0px;">
                                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" Height="27px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txt_roll" runat="server" Style="margin-left: 5px; height: 20px;
                                                                width: 100px;" placeholder="Search"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </fieldset>
                                                        <fieldset id="divdt" runat="server" visible="false" style="margin-left: 263px; width: 222px;
                                                            height: 20px; margin-top: -42px;">
                                                            <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                                                            <asp:RadioButton ID="rb_common" runat="server" Text="Common" Width="90px" GroupName="s2"
                                                                OnCheckedChanged="rb_common_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_detail" runat="server" Text="Detail" Width="61px" GroupName="s2"
                                                                Checked="true" OnCheckedChanged="rb_detail_Change" AutoPostBack="true" />
                                                        </fieldset>
                                                        <fieldset id="fsdailydetail" runat="server" visible="false" style="margin-left: 220px;
                                                            width: 99px; height: 16px; margin-top: -38px;">
                                                            <asp:CheckBox ID="chkdailydetail" runat="server" Text="Daily Details" OnCheckedChanged="chkdailydetail_chkdailydetail"
                                                                AutoPostBack="true" />                                                           
                                                        </fieldset>
                                                    </div>
                                                </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <div id="divstudwise" runat="server" visible="false">
                                                            <fieldset style="width: 195px; height: 16px; margin-top: 0px;">
                                                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" Height="27px"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_roll" runat="server" Style="margin-left: 5px; height: 20px;
                                                                    width: 100px;" placeholder="Search"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </fieldset>
                                                            <fieldset id="divdt" runat="server" visible="false" style="margin-left: 263px; width: 222px;
                                                                height: 20px; margin-top: -42px;">
                                                                <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                                                                <asp:RadioButton ID="rb_common" runat="server" Text="Common" Width="90px" GroupName="s2"
                                                                    OnCheckedChanged="rb_common_Change" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rb_detail" runat="server" Text="Detail" Width="61px" GroupName="s2"
                                                                    Checked="true" OnCheckedChanged="rb_detail_Change" AutoPostBack="true" />
                                                            </fieldset>
                                                            <fieldset id="fsdailydetail" runat="server" visible="false" style="margin-left: 220px;
                                                                width: 110px; height: 16px; margin-top: -38px;">
                                                                <asp:CheckBox ID="chkdailydetail" runat="server" Text="Daily Details" OnCheckedChanged="chkdailydetail_chkdailydetail"
                                                                    AutoPostBack="true" />
                                                                <%--  <asp:RadioButton ID="rbdailydetail" runat="server" Text="Daily Deails" OnCheckedChanged="rbdailydetail_OnCheckedChanged" AutoPostBack="true" />--%>
                                                            </fieldset>
                                                        </div>
                                                    </td>
                                                    <td colspan="4">
                                                        <div id="studwise" runat="server" visible="false">
                                                            <fieldset id="fspaid" runat="server" visible="false" style="margin-left: -56px; margin-top: -1px;
                                                                height: 16px; width: 275px;">
                                                                <asp:Label ID="lblpaid" runat="server" Text="Paid Details"></asp:Label>
                                                                <asp:CheckBox ID="chkpaid" runat="server" AutoPostBack="true" OnCheckedChanged="chkpaid_OnCheckedChanged" />
                                                                <asp:RadioButton ID="rb_paid" runat="server" Text="Paid" Enabled="false" Width="56px"
                                                                    GroupName="p1" OnCheckedChanged="rb_paid_OnCheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="tobepaid" runat="server" Text="Yet TobePaid" Enabled="false"
                                                                    Width="107px" GroupName="p1" OnCheckedChanged="tobepaid_OnCheckedChanged" AutoPostBack="true" />
                                                            </fieldset>
                                                            <fieldset id="fshor" runat="server" visible="false" style="margin-left: 329px; margin-top: -33px;
                                                                height: 12px; width: 487px;">
                                                                <asp:RadioButton ID="rb_hori" runat="server" Text="Horizontalwise" Width="120px"
                                                                    GroupName="s3" Checked="true" OnCheckedChanged="rb_hori_Change" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rb_vert" runat="server" Text="Verticalwise" Width="120px" GroupName="s3"
                                                                    Checked="true" OnCheckedChanged="rb_vert_Change" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbstudhed" runat="server" Text="header" Width="80px" GroupName="s4"
                                                                    Checked="true" OnCheckedChanged="rbstudhed_Change" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbstudled" runat="server" Text="Ledger" Width="80px" GroupName="s4"
                                                                    OnCheckedChanged="rbstudled_Change" AutoPostBack="true" />
                                                            </fieldset>
                                                        </div>
                                                        <div id="pagelnk" runat="server" visible="false" style="margin-left: 852px; width: 75px;
                                                            margin-top: -26px; position: absolute;">
                                                            <asp:LinkButton ID="LinkButton3" runat="server" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Small" ForeColor="Blue" CausesValidation="False" OnClick="btn_pageset_Click">Page Setting</asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="10">
                                                        <div id="studpaid" runat="server" visible="false">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:RadioButton ID="rb_header" runat="server" Text="Header" Width="80px" GroupName="s1"
                                                            Checked="true" OnCheckedChanged="rb_header_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_ledger" runat="server" Text="Ledger" Width="80px" GroupName="s1"
                                                            OnCheckedChanged="rb_ledger_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_batch" runat="server" Text="Batch Year" Width="100px" GroupName="s1"
                                                            OnCheckedChanged="rb_batch_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_degree" runat="server" Text="Degree" Width="80px" GroupName="s1"
                                                            OnCheckedChanged="rb_degree_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_dept" runat="server" Text="Department" Width="130px" GroupName="s1"
                                                            OnCheckedChanged="rb_dept_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_sem" runat="server" Text="Semester" Width="100px" GroupName="s1"
                                                            OnCheckedChanged="rb_sem_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_studentwise" runat="server" Text="StudentWise" Width="110px"
                                                            GroupName="s1" OnCheckedChanged="rb_studwise_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_others" runat="server" Text="Others" Width="70px" GroupName="s1"
                                                            OnCheckedChanged="rb_others_Change" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rb_total" runat="server" Text="Total" Width="70px" GroupName="s1"
                                                            OnCheckedChanged="rb_total_OnCheckedChanged" AutoPostBack="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                                            OnClientClick="return checkDate()" OnClick="btnsearch_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblfyear" Text="Finance Year"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtfyear" Style="height: 20px; width: 150px;" CssClass="Dropdown_Txt_Box"
                                                                    runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                                <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="200px">
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
                                                    <%-- <td colspan="2">
                                                    <asp:CheckBox ID="chkinclude" runat="server" Text=" Include Discontinue" />
                                                </td>--%>
                                                    <%-- <td>
                                                    Include Discontinue
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkinclude" runat="server" Text=" Include Discontinue" Visible="false" />
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtinclude" Style="height: 20px; width: 115px;" CssClass="Dropdown_Txt_Box"
                                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel" Width="147px">
                                                                <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" OnCheckedChanged="cbinclude_OnCheckedChanged"
                                                                    AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cblinclude" runat="server" OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged"
                                                                    AutoPostBack="True">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                                PopupControlID="pnlinclude" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                                    <td colspan="2" id="deptfld" runat="server" visible="false">
                                                        <fieldset id="deptcomm" runat="server" visible="false" style="height: 9px; width: 205px;">
                                                            <asp:RadioButton ID="rbdeptcommon" runat="server" Text="Common" Width="100px" GroupName="dc1"
                                                                Checked="true" OnCheckedChanged="rbdeptcommon_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbdeptdetail" runat="server" Text="Detail" Width="100px" GroupName="dc1"
                                                                OnCheckedChanged="rbdeptdetail_Change" AutoPostBack="true" />
                                                        </fieldset>
                                                    </td>
                                                    <td colspan="5" id="deptdtfld" runat="server" visible="false">
                                                        <fieldset id="deptdetail" runat="server" visible="false" style="height: 9px; width: 315px;
                                                            float: left;">
                                                            <asp:RadioButton ID="rbdeptyear" runat="server" Text="Year" Width="100px" GroupName="dd1"
                                                                Checked="true" OnCheckedChanged="rbdeptyear_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbdepthead" runat="server" Text="Header" Width="100px" GroupName="dd1"
                                                                OnCheckedChanged="rbdepthead_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbdeptledg" runat="server" Text="Ledger" Width="100px" GroupName="dd1"
                                                                OnCheckedChanged="rbdeptledg_Change" AutoPostBack="true" />
                                                        </fieldset>
                                                    </td>
                                                    <td colspan="7" id="otherfld" runat="server" visible="false">
                                                        <fieldset id="otherdetail" runat="server" visible="false" style="height: 9px; width: 315px;
                                                            float: left;">
                                                            <asp:RadioButton ID="rbstaff" runat="server" Text="Staff" Width="100px" GroupName="dd2"
                                                                Checked="true" OnCheckedChanged="rbstaff_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbvendor" runat="server" Text="Vendor" Width="100px" GroupName="dd2"
                                                                OnCheckedChanged="rbvendor_Change" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbothers" runat="server" Text="Others" Width="100px" GroupName="dd2"
                                                                OnCheckedChanged="rbothers_Change" AutoPostBack="true" />
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Label ID="lbl_error1" runat="server" Visible="false"></asp:Label>
                                    </center>
                                    <br />
                                </td>
                            </tr>
                            <tr id="trdiv" runat="server" style="float: left;">
                                <td>
                                    <%-- ***--%>
                                    <center>
                                        <div>
                                            <asp:Label ID="lblhed" runat="server" Text="Header" Visible="false" Font-Size="Medium"
                                                Font-Bold="True" Font-Names="Book Antiqua" ForeColor="Blue" />
                                            <br />
                                            <center>
                                                <div>
                                                    <Insproplus:printmaster runat="server" ID="Printmaster12" Visible="false" />
                                                    <center>
                                                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" Visible="false"
                                                            Height="22px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; float: left;">
                                                            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                        </asp:Panel>
                                                    </center>
                                                </div>
                                                <br />
                                                <br />
                                                <div>
                                                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="930px">
                                                        <div id="divcolumn" runat="server" style="height: 87px; width: 930px;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small;" Visible="false"
                                                                            Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="tborder" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </center>
                                            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                                                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                                                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                                ExpandedImage="~/images/down.jpeg">
                                            </asp:CollapsiblePanelExtender>
                                            <br />
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Button ID="Button1" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Bank Statement Print" OnClick="btnprintmasterButton1_Click"
                                                Height="32px" CssClass="textbox textbox1" Width="172px" />
                                        </div>
                                    </center>
                                </td>
                            </tr>
                            <tr id="trchart" runat="server" style="float: left;">
                                <td>
                                    <br />
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblchart" runat="server" Visible="false" Style="color: Blue;"></asp:Label>
                                            <div>
                                                <asp:Panel ID="pnlchart" runat="server">
                                                    <asp:Chart ID="chart" runat="server" Visible="false">
                                                        <%--OnClick="chart_OnClick"--%>
                                                        <Series>
                                                            <%-- <asp:Series Name="Series1" PostBackValue="#VALX">
                                                        </asp:Series>--%>
                                                        </Series>
                                                        <Legends>
                                                            <asp:Legend Title="Performance Graph" Font="Book Antiqua">
                                                            </asp:Legend>
                                                        </Legends>
                                                        <ChartAreas>
                                                            <asp:ChartArea Name="ChartArea1">
                                                                <AxisY LineColor="White">
                                                                    <LabelStyle Font="Trebuchet MS, 15pt" />
                                                                    <MajorGrid LineColor="#e6e6e6" />
                                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                                </AxisY>
                                                                <AxisX LineColor="White">
                                                                    <LabelStyle Font="Trebuchet MS,15pt" />
                                                                    <MajorGrid LineColor="#e6e6e6" />
                                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                                </AxisX>
                                                            </asp:ChartArea>
                                                        </ChartAreas>
                                                        <Legends>
                                                        </Legends>
                                                    </asp:Chart>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel ID="pnl" runat="server">
                                        <div id="header" runat="server" visible="false" style="overflow: auto;">
                                            <div id="div1" runat="server" visible="true" style="width: 961px; overflow: auto;
                                                background-color: White; border-radius: 10px;">
                                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                                                    BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                    class="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchanged">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <center>
                                        <%--<asp:Button ID="btn_fpled" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btn_fpled_Click" Height="30px" Style="margin-top: -88px;
                            margin-left: -560px;" CssClass="textbox textbox1" Width="73px" />--%>
                                        <asp:UpdatePanel ID="val1" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <br />
                                                    <div id="ledger" runat="server">
                                                        <asp:Label ID="lbl_ledg" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                        <br />
                                                        <%-- <center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnlledgercolorder" runat="server" CssClass="cpHeader" Visible="false"
                                                        Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lbl_ledgercolorder" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                        
                                                        <Insproplus:printmaster runat="server" ID="Printmaster6" Visible="false" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Button ID="btn_ledgercolor" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Go" OnClick="btn_ledgercolor_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <asp:Panel ID="Pnlledcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                    Width="930px">
                                                    <div id="div2" runat="server" style="height: 87px; width: 930px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_ledgercol" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_ledgercol_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_ledgercol" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px" OnClick="lnk_ledgercol_Click">Remove  All</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_ledgercol" Visible="false" Width="867px" TextMode="MultiLine"
                                                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkl_ledgercolorder" runat="server" Height="43px" Width="850px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_ledgercolorder_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_ledcellclick" runat="server" Text="Serach" Style="margin-top: -12px;
                                                                        margin-left: -17px;" OnClick="btn_ledcellclick_Clcik" CssClass="textbox btn2" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Pnlledcolorder"
                                            CollapseControlID="pnlledgercolorder" ExpandControlID="pnlledgercolorder" Collapsed="true"
                                            TextLabelID="lbl_ledgercolorder" CollapsedSize="0" ImageControlID="Imagefilter"
                                            CollapsedImage="~/images/right.jpeg" ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>--%>
                                                        <br />
                                                        <div id="divledger" runat="server" visible="false" style="width: 950px; height: 275px;
                                                            overflow: auto; background-color: White; border-radius: 10px;">
                                                            <FarPoint:FpSpread ID="fpledger" runat="server" Visible="false" BorderStyle="Solid"
                                                                BorderWidth="0px" Width="930px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder" OnCellClick="fpledger_OnCellClick" OnPreRender="fpledger_Selectedindexchanged">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                            <%-- <center>
                                                <div id="printled" runat="server" visible="false">
                                                    <asp:Button ID="btnprintmasterled" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                                                </div>
                                            </center>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </center>
                                    <br />
                                    <center>
                                        <div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <br />
                                                    <div id="batch" runat="server">
                                                        <asp:Label ID="lbl_bat" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                        <%-- <asp:Button ID="btn_fpbat" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btn_fpbat_Click" Height="30px" Style="margin-top: 18px;
                                            position: absolute; margin-left: -473px;" CssClass="textbox textbox1" Width="73px" />--%>
                                                        <br />
                                                        <%-- <center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnl_batchcolorder" runat="server" CssClass="cpHeader" Visible="false"
                                                        Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lbl_batcolorder" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                       
                                                        <Insproplus:printmaster runat="server" ID="Printmaster7" Visible="false" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Button ID="btn_batcolorder" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Go" OnClick="btn_batcolorder_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <asp:Panel ID="pnl_batcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                    Width="930px">
                                                    <div id="div3" runat="server" style="height: 87px; width: 930px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_batcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_batcolorder_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_batcolorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px" OnClick="lnk_batcolorder_Click">Remove  All</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_batcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkl_batchcolorder" runat="server" Height="43px" Width="850px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_batchcolorder_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_batchcellclick" runat="server" Text="Serach" Style="margin-top: 10px;
                                                                        margin-left: -17px;" OnClick="btn_batchcellclick_Clcik" CssClass="textbox btn2" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pnl_batcolorder"
                                            CollapseControlID="pnl_batchcolorder" ExpandControlID="pnl_batchcolorder" Collapsed="true"
                                            TextLabelID="lbl_batcolorder" CollapsedSize="0" ImageControlID="Imagefilter"
                                            CollapsedImage="~/images/right.jpeg" ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>--%>
                                                        <br />
                                                        <div id="divbatch" runat="server" visible="false" style="width: 950px; height: 275px;
                                                            overflow: auto; background-color: White; border-radius: 10px;">
                                                            <FarPoint:FpSpread ID="fpbatch" runat="server" Visible="false" BorderStyle="Solid"
                                                                BorderWidth="0px" Width="930px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder" OnCellClick="fpbatch_OnCellClick" OnPreRender="fpbatch_Selectedindexchanged">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                            <%-- <center>
                                                <div id="printbat" runat="server" visible="false">
                                                    <asp:Button ID="printmasterbat" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printmaster2" Visible="false" />
                                                </div>
                                            </center>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <br />
                                                    <div id="divdegr" runat="server">
                                                        <asp:Label ID="lbl_degr" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                        <%-- <asp:Button ID="btn_fpdeg" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btn_fpdeg_Click" Height="30px" Style="margin-top: 18px;
                                            margin-left: -517px; position: absolute;" CssClass="textbox textbox1" Width="73px" />--%>
                                                        <br />
                                                        <%-- <center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnl_degcolorder" runat="server" CssClass="cpHeader" Visible="false"
                                                        Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lbl_degcolorder" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                      
                                                        <Insproplus:printmaster runat="server" ID="Printmaster8" Visible="false" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Button ID="btn_degcolorder" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Go" OnClick="btn_degcolorder_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <asp:Panel ID="pnl_degreecolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                    Width="930px">
                                                    <div id="div4" runat="server" style="height: 87px; width: 930px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_degcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_degcolorder_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_degcolorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px" OnClick="lnk_degcolorder_Click">Remove  All</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_degcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkl_degcolorder" runat="server" Height="43px" Width="850px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_degcolorder_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_degreecellclick" runat="server" Text="Serach" Style="margin-top: 10px;
                                                                        margin-left: -17px;" OnClick="btn_degreecellclick_Clcik" CssClass="textbox btn2" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnl_degreecolorder"
                                            CollapseControlID="pnl_degcolorder" ExpandControlID="pnl_degcolorder" Collapsed="true"
                                            TextLabelID="lbl_degcolorder" CollapsedSize="0" ImageControlID="Imagefilter"
                                            CollapsedImage="~/images/right.jpeg" ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>--%>
                                                        <br />
                                                        <div id="divdegree" runat="server" visible="false" style="width: 950px; height: 275px;
                                                            overflow: auto; background-color: White; border-radius: 10px;">
                                                            <FarPoint:FpSpread ID="fpdegree" runat="server" Visible="false" BorderStyle="Solid"
                                                                BorderWidth="0px" Width="1000px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder" OnCellClick="fpdegree_OnCellClick" OnPreRender="fpdegree_Selectedindexchanged">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                            <%-- <center>
                                                <div id="printdeg" runat="server" visible="false">
                                                    <asp:Button ID="printmasterdeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printmaster3" Visible="false" />
                                                </div>
                                            </center>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <br />
                                                    <div id="divdep" runat="server">
                                                        <asp:Label ID="lbl_dep" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                        <%-- <asp:Button ID="btn_fpdep" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btn_fpdep_Click" Height="30px" Style="margin-top: 18px;
                                            margin-left: -574px; position: absolute;" CssClass="textbox textbox1" Width="73px" />--%>
                                                        <br />
                                                        <%--<center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnl_deptcolord" runat="server" CssClass="cpHeader" Visible="false"
                                                        Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lbl_deptcolorder" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />                                                       
                                                        <Insproplus:printmaster runat="server" ID="Printmaster9" Visible="false" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Button ID="btn_deptcolorder" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Go" OnClick="btn_degcolorder_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <asp:Panel ID="pnl_deptcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                    Width="930px">
                                                    <div id="div5" runat="server" style="height: 107px; width: 930px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_deptcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_deptcolorder_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_deptcolorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px" OnClick="lnk_deptcolorderr_Click">Remove  All</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_deptcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkl_deptcolorder" runat="server" Height="43px" Width="850px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_deptcolorder_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_deptcellclick" runat="server" Text="Serach" Style="margin-top: 10px;
                                                                        margin-left: -17px;" OnClick="btn_deptcellclick_Clcik" CssClass="textbox btn2" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="pnl_deptcolorder"
                                            CollapseControlID="pnl_deptcolord" ExpandControlID="pnl_deptcolord" Collapsed="true"
                                            TextLabelID="lbl_deptcolorder" CollapsedSize="0" ImageControlID="Imagefilter"
                                            CollapsedImage="~/images/right.jpeg" ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>--%>
                                                        <br />
                                                        <div id="divdept" runat="server" visible="false" style="width: 950px; height: 275px;
                                                            overflow: auto; background-color: White; border-radius: 10px;">
                                                            <FarPoint:FpSpread ID="fpdept" runat="server" Visible="false" BorderStyle="Solid"
                                                                BorderWidth="0px" Width="1200px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder" OnCellClick="fpdept_OnCellClick" OnPreRender="fpdept_Selectedindexchanged">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                            <%--<center>
                                                <div id="printdept" runat="server" visible="false">
                                                    <asp:Button ID="printmasterdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printmaster4" Visible="false" />
                                                </div>
                                            </center>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </center>
                                    <br />
                                    <center>
                                        <div>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <br />
                                                    <div id="divsema" runat="server">
                                                        <asp:Label ID="lbl_sem" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                        <%--   <asp:Button ID="btn_fpsema" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btn_fpsema_Click" Height="30px" Style="margin-top: 18px;
                                            margin-left: -711px; position: absolute;" CssClass="textbox textbox1" Width="73px" />--%>
                                                        <br />
                                                        <%--<center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnl_semcolord" runat="server" CssClass="cpHeader" Visible="false"
                                                        Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lbl_semcolor" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                      
                                                        <Insproplus:printmaster runat="server" ID="Printmaster10" Visible="false" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Button ID="btn_semcolorder" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Go" OnClick="btn_semcolorder_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <asp:Panel ID="pnl_semcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                    Width="930px">
                                                    <div id="div6" runat="server" style="height: 107px; width: 930px;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_colorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_colorders_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_semcolorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px" OnClick="lnk_semcolorder_Click">Remove  All</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_semcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkl_semcolorder" runat="server" Height="43px" Width="850px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_semcolorder_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_semcellclick" runat="server" Text="Serach" Style="margin-top: 10px;
                                                                        margin-left: -17px;" OnClick="btn_semcellclick_Clcik" CssClass="textbox btn2" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>--%>
                                                        <%--<asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="pnl_semcolorder"
                                            CollapseControlID="pnl_semcolord" ExpandControlID="pnl_semcolord" Collapsed="true"
                                            TextLabelID="lbl_semcolor" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                            ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>--%>
                                                        <br />
                                                        <div id="divsem" runat="server" visible="false" style="width: 950px; height: 300;
                                                            overflow: auto; background-color: White; border-radius: 10px;">
                                                            <FarPoint:FpSpread ID="fpsem" runat="server" Visible="false" BorderStyle="Solid"
                                                                BorderWidth="0px" Width="1300px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder" OnCellClick="fpsem_OnCellClick" OnPreRender="fpsem_Selectedindexchanged">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                            <%--<center>
                                                <div id="printsem" runat="server" visible="false">
                                                    <asp:Button ID="printmastersem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printmaster5" Visible="false" />
                                                </div>
                                            </center>--%>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <br />
                                        <center>
                                            <div>
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <div id="div7" runat="server">
                                                            <asp:Label ID="lbl_semcell" runat="server" Text="" Style="color: Blue;" Font-Bold="true"></asp:Label>
                                                            <br />
                                                            <%--<center>
                                                <div>
                                                    <center>
                                                        <asp:Panel ID="pnl_studcolord" runat="server" CssClass="cpHeader" Visible="false"
                                                            Height="29px" Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                            <asp:Label ID="lbl_studcolord" Text="Column Order" runat="server" Font-Size="Medium"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                            <asp:Button ID="btn_fpstud" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="Print" OnClick="btn_fpstud_Click" Height="30px" Style="margin-top: -27px;
                                                                margin-left: 152px;" CssClass="textbox textbox1" Width="73px" />
                                                            <Insproplus:printmaster runat="server" ID="Printmaster11" Visible="false" />
                                                        </asp:Panel>
                                                    </center>
                                                </div>
                                                <br />
                                                <div>
                                                    <asp:Panel ID="pnl_studcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                        Width="930px">
                                                        <div id="div8" runat="server" style="height: 107px; width: 930px;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chk_studcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chk_studcolorder_CheckedChanged" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="lnk_studcolorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                            Visible="false" Width="111px" OnClick="lnk_studcolorder_Click">Remove  All</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_studcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                            CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBoxList ID="chkl_studcolorder" runat="server" Height="43px" Width="850px"
                                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="chkl_studcolorder_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btn_studcolorder" runat="server" Text="Serach" Style="margin-top: 10px;
                                                                            margin-left: -17px;" OnClick="btn_studcolorder_Clcik" CssClass="textbox btn2" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </center>
                                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="pnl_studcolorder"
                                                CollapseControlID="pnl_studcolord" ExpandControlID="pnl_studcolord" Collapsed="true"
                                                TextLabelID="lbl_studcolord" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                                ExpandedImage="~/images/down.jpeg">
                                            </asp:CollapsiblePanelExtender>--%>
                                                            <br />
                                                            <div id="divstudcell" runat="server" visible="false" style="width: 950px; height: 300px;
                                                                overflow: auto; background-color: White; border-radius: 10px;">
                                                                <FarPoint:FpSpread ID="fpstudcell" runat="server" Visible="false" BorderStyle="Solid"
                                                                    BorderWidth="0px" Style="overflow: auto; cursor: grab; border: 0px solid #999999;
                                                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                    class="spreadborder" OnCellClick="fpstudcell_OnCellClick" OnPreRender="fpstudcell_Selectedindexchanged">
                                                                    <Sheets>
                                                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                                        </FarPoint:SheetView>
                                                                    </Sheets>
                                                                </FarPoint:FpSpread>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </center>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>--%>
                                        <center>
                                            <div id="rptprint" runat="server" visible="false">
                                                <%--   style="margin-top:-197px; margin-left:10px;" >--%>
                                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
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
                                        <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div id="pageset" runat="server" class="popupstyle" visible="false" style="height: 60em;
                                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                                top: 15px; left: 0;">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 434px;"
                                    OnClick="imagebtnpopsscode_Click" />
                                <center>
                                    <div style="background-color: White; height: 719px; width: 900px; border: 5px solid #0CA6CA;
                                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                        <center>
                                            <div>
                                                <span class="fontstyleheader" style="color: Green;">Page Settings</span></div>
                                            <br />
                                        </center>
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_selpag" runat="server" Visible="false" Text=" Select Page Tittle"
                                                            Style="margin-top: 10px; margin-left: 10px;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_plus" runat="server" Visible="false" Text="+" CssClass="textbox btn"
                                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click"
                                                            Height="33px" Width="35px" />
                                                        <asp:DropDownList ID="ddl_group" runat="server" Visible="false" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btn_minus" runat="server" Visible="false" Text="-" Font-Bold="true"
                                                            Font-Size="Medium" Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox btn"
                                                            OnClick="btnminus_Click" />
                                                    </td>
                                                    <td>
                                                        Text
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_pgtxt" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt" runat="server" TargetControlID="txt_pgtxt"
                                                            FilterType="Custom,LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_pgtxt"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_str2" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_str" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnl_str" runat="server" CssClass="multxtpanel " Style="width: 121px;
                                                            height: 100px;">
                                                            <asp:CheckBox ID="chk_str" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="chk_str_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="chkl_str" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_str_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_str"
                                                            PopupControlID="pnl_str" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Select Course
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chk_course" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="chk_course_OnCheckedChanged" />
                                                        <div style="height: 100px; width: 300px; overflow: auto; ackground-color: White;
                                                            border-radius: 10px; border: 1px solid #c4c4c4;">
                                                            <asp:Panel ID="pnl_course" runat="server">
                                                                <%-- <asp:CheckBox ID="chk_course" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_course_OnCheckedChanged" />--%>
                                                                <asp:CheckBoxList ID="chkl_course" runat="server" OnSelectedIndexChanged="chkl_course_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Select Semester
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chk_sema" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="chk_sema_OnCheckedChanged" />
                                                        <div style="height: 100px; width: 300px; overflow: auto; ackground-color: White;
                                                            border-radius: 10px; border: 1px solid #c4c4c4;">
                                                            <asp:Panel ID="pnl_sema" runat="server">
                                                                <%-- <asp:CheckBox ID="chk_sema" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_sema_OnCheckedChanged" />--%>
                                                                <asp:CheckBoxList ID="chkl_sema" runat="server" OnSelectedIndexChanged="chkl_sema_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%-- <asp:Label ID="lbl_slcthdr" runat="server" Text="Select Header"></asp:Label>--%>
                                                        Select Header
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="lb_selecthdr" runat="server" Height="100px" Width="200px"></asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <center>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox btn" OnClick="btnMvOneRt_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox btn" OnClick="btnMvTwoRt_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox btn" OnClick="btnMvOneLt_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox btn" OnClick="btnMvTwoLt_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </center>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:ListBox ID="lb_hdr" runat="server" Height="150px" Width="200px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Select Fields
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chk_filed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="chk_field_OnCheckedChanged" />
                                                        <div style="height: 100px; width: 300px; overflow: auto; ackground-color: White;
                                                            border-radius: 10px; border: 1px solid #c4c4c4;">
                                                            <asp:Panel ID="pnl_filed" runat="server">
                                                                <%-- <asp:CheckBox ID="chk_filed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_field_OnCheckedChanged" />--%>
                                                                <asp:CheckBoxList ID="chkl_field" runat="server" OnSelectedIndexChanged="chkl_filed_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        TotalWise
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chk_total" runat="server" AutoPostBack="true" OnCheckedChanged="chk_total_OnCheckedChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                        <div style="margin-left: 10px; margin-top: 28px;">
                                            <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2" />
                                            <asp:Button ID="btn_del" runat="server" Visible="false" Text="Delete" OnClick="btn_del_Click"
                                                CssClass="textbox btn2" />
                                            <asp:Button ID="btn_exit" runat="server" Text="Exit" OnClick="btn_exit_Click" CssClass="textbox btn2" />
                                        </div>
                                    </div>
                                </center>
                                <%-- <div>
                        <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2" />
                        <asp:Button ID="btn_del" runat="server" Text="Delete" OnClick="btn_del_Click" CssClass="textbox btn2" />
                        <asp:Button ID="btn_exit" runat="server" Text="Exit" OnClick="btn_exit_Click" CssClass="textbox btn2" />
                    </div>--%>
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
                    </div>
                </div>
            </center>
            <center>
                <table id="studtype" runat="server" visible="false">
                    <tr>
                        <td>
                            Student
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_student" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stud" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_stud" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cb_stud" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_stud_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_stud" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stud_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_stud"
                                        PopupControlID="panel_stud" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_setype" runat="server" Text="Seat Type" Style="margin-top: -12px;
                                margin-left: -4px; position: absolute;"></asp:Label>
                            <%-- Seat Type--%>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_seattype" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_seat" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_seat" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 150px;">
                                        <asp:CheckBox ID="cb_seat" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_seat_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_seat"
                                        PopupControlID="panel_seat" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stud_id" runat="server" Text="Student Type" Style="margin-top: -12px;
                                margin-left: -4px; position: absolute;"></asp:Label>
                            <%--  Student Type--%>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_studtype" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studtype" runat="server" Style="height: 20px; width: 100px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_studtype" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 106px; height: 100px;">
                                        <asp:CheckBox ID="cb_studtype" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_studtype_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_studtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_studtype_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_studtype"
                                        PopupControlID="panel_studtype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
