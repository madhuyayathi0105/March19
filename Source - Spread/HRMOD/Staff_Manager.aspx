<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_Manager.aspx.cs" Inherits="Staff_Manager" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/Commonfilter.ascx" TagName="Search" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <%--    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script src="../Scripts/jquery-latest.min.js" type="text/javascript"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .btn_class1
        {
            font-weight: bold;
            margin-left: 0px;
            font-family: book antiqua;
            font-size: medium;
            background-color: MediumSlateBlue;
            border: 0 none;
            border-radius: 20px 0;
            color: white;
            font-family: trebuchet ms;
            font-size: medium;
            font-weight: bold;
            height: 30px;
            margin-left: 0;
            transition: all 0.6s ease-out 0s;
        }

        .btn_class2
        {
            border: 0 none;
            border-radius: 20px 0;
            color: white;
            font-family: trebuchet ms;
            font-size: medium;
            font-weight: bold;
            height: 30px;
            margin-left: 0;
            transition: all 0.6s ease-out 0s;
        }

        .txtlower
        {
            text-transform: lowercase;
        }

        .txtupper
        {
            text-transform: uppercase;
        }

        .txtcapitalize
        {
            text-transform: capitalize;
        }

        .txtnone
        {
            text-transform: none;
        }

        .email
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
    </style>
    <body>
        <script type="text/javascript">
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lblValStudRep.ClientID %>').innerHTML = "";
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }
            function lblvis() {
                document.getElementById('<%=lblstf.ClientID %>').innerHTML = "";
                document.getElementById('<%=lblstfalert.ClientID %>').innerHTML = "";
            }
            function onvenchange() {
                document.getElementById('<%=lbl_alertc.ClientID %>').innerHTML = "";
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
                fromDate = document.getElementById('<%=txt_adate.ClientID%>').value;
                toDate = document.getElementById('<%=txt_appdoj.ClientID%>').value;

                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);

                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
                var today = new Date();
                var getmonth = (today.getMonth() + 1).toString();
                var len = getmonth.length;
                var currmonth = "0";
                if (parseInt(len) == 1) {
                    currmonth = "0" + getmonth;
                }
                else {
                    currmonth = getmonth;
                }
                var getday = (today.getDate()).toString();
                var leng = getday.length;
                var currday = "0";
                if (parseInt(leng) == 1) {
                    currday = "0" + getday;
                }
                else {
                    currday = getday;
                }
                var currentDate = currday + '/' + currmonth + '/' + today.getFullYear();

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
                    document.getElementById('<%=txt_appdoj.ClientID%>').value = currentDate;
                    alert("Join Date should be greater than the Appointed Date ");
                    return false;
                }
            }

            function chkdedamnt() {
                var loanamnt = document.getElementById('<%=txt_lamt.ClientID %>').value;
                var monthcount = document.getElementById('<%=txt_tenure.ClientID %>').value;
                var amntval = 0.0;
                if (parseFloat(loanamnt) != 0.0 && parseFloat(monthcount) != 0.0) {
                    if (parseFloat(loanamnt) > parseFloat(monthcount)) {
                        amntval = (parseFloat(loanamnt) / parseFloat(monthcount));
                        document.getElementById('<%=txt_dedamt.ClientID %>').value = Math.round(amntval);
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
                var txtval = document.getElementById("<%=txt_percentage.ClientID %>").value;
                if (txtval.trim() != "") {
                    if (parseFloat(txtval) > 100) {
                        document.getElementById('<%=txt_percentage.ClientID %>').value = "";
                    }
                }
            }

            function checkEmail(id) {
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Staff Manager</span>
                </div>
            </center>
        </div>
        <center>
            <fieldset style="background-color: #4682B4; border: 1px solid #ccc; box-shadow: 0 0 8px #999999; height: 0px; margin-left: 0px; margin-top: 0px; padding: 1em; margin-left: 0px; width: 1000px;">
            </fieldset>
            <table style="margin-left: 0px;">
                <tr>
                    <td>
                        <asp:Button ID="btn_application" Text="Application" Visible="false" runat="server"
                            CssClass="btn_class1" OnClick="btn_application_OnClick" />
                        <asp:Button ID="btn_cletter" Text="Call Letter" Visible="false" runat="server" OnClick="btn_cletter_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_selection" Text="Selection" Visible="false" runat="server" OnClick="btn_selection_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_appointment" Text="Appointment" Visible="false" runat="server"
                            OnClick="btn_appointment_OnClick" CssClass="btn_class1" />
                        <asp:Button ID="btn_transfer" Text="Transfer" Visible="false" runat="server" OnClick="btn_transfer_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_relieve" Text="Relieve" Visible="false" runat="server" OnClick="btn_relieve_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_appraisal" Text="Appraisal" Visible="false" runat="server" OnClick="btn_appraisal_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_promotion" Text="Promotion" Visible="false" runat="server" OnClick="btn_promotion_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_increment" Text="Increment" Visible="false" runat="server" OnClick="btn_increment_OnClick"
                            CssClass="btn_class1" />
                        <asp:Button ID="btn_loan" Text="Staff Loan" Visible="false" runat="server" OnClick="btn_loan_OnClick"
                            CssClass="btn_class1" />
                    </td>
                </tr>
            </table>
            <fieldset style="background-color: #4682B4; border: 1px solid #ccc; box-shadow: 0 0 8px #999999; height: 0px; margin-left: 0px; margin-top: 0px; padding: 1em; margin-left: 0px; width: 1000px;">
            </fieldset>
            <div>
                <asp:Label ID="headermain" Text="" runat="server" Visible="false" class="fontstyleheader"
                    Style="color: indigo;"></asp:Label>
            </div>
            <center>
                <fieldset id="staff" runat="server" style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: 0px; width: 924px;">
                    <center>
                        <table id="tab1" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College Name : " Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="120px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="310px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldep" runat="server" Text="Department : " Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 250px;">
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
                                    <asp:Label ID="lbl_desig" runat="server" Text="Designation :" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 250px;">
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
                            </tr>
                        </table>
                        <table id="tab2" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stype" runat="server" Text="Staff Type :" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 110px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                <td>
                                    <asp:CheckBox ID="searchbydate" runat="server" AutoPostBack="true" OnCheckedChanged="searchbydate_OnCheckedChanged"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="Search By" />
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lbl_fdate" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_fdate" runat="server" AutoPostBack="true" OnTextChanged="txt_fdate_OnTextChanged"
                                        Enabled="false" CssClass="textbox txtheight2" Width="90px" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_fdate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbl_tdate" runat="server" Text="To " Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_tdate" runat="server" AutoPostBack="true" OnTextChanged="txt_tdate_OnTextChanged"
                                        Enabled="false" CssClass="textbox txtheight2" Width="90px" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_tdate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td width="35px">
                                    <asp:Label ID="lbladdstatus" runat="server" Text="Status" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td width="136px">
                                    <asp:UpdatePanel ID="updaddstatus" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtaddstatus" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnladdstatus" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;"
                                                Width="135px">
                                                <asp:CheckBox ID="cbaddstatus" runat="server" Text="Select All" OnCheckedChanged="cbaddstatus_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbladdstatus" runat="server" OnSelectedIndexChanged="cbladdstatus_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtaddstatus"
                                                PopupControlID="pnladdstatus" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlsearchappstf" runat="server" CssClass="textbox1 ddlheight2"
                                        OnSelectedIndexChanged="ddlsearchappstf_change" AutoPostBack="true" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtappstfapplcode" runat="server" Visible="false" MaxLength="15"
                                        AutoPostBack="true" OnTextChanged="txtappstfapplcode_change" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:TextBox ID="txtappstfcode" runat="server" Visible="false" MaxLength="15" AutoPostBack="true"
                                        OnTextChanged="txtappstfcode_change" CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="autocomextappapplno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetapplCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtappstfapplcode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <asp:AutoCompleteExtender ID="autocomextappscode" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtappstfcode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblappstfname" runat="server" Text="Staff Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtappstfname" runat="server" MaxLength="100" AutoPostBack="true"
                                        OnTextChanged="txtappstfname_change" CssClass="textbox txtheight2 txtcapitalize"
                                        Style="width: 150px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtappstfname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="rel" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Button ID="btn_appgo" Text="Go" runat="server" OnClick="btn_appgo_Click" Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_new" Text="Add New" runat="server" OnClick="btn_new_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_intcallgo" Text="Go" runat="server" Visible="false" OnClick="btn_intcallgo_Click"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_intcallnew" Text="Add New" runat="server" Visible="false" OnClick="btn_intcallnew_Click"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_selectgo" Text="Go" runat="server" Visible="false" OnClick="btn_selectgo_Click"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_selectnew" Text="Add New" runat="server" Visible="false" OnClick="btn_selectnew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_aptgo" Text="Go" runat="server" Visible="false" OnClick="btn_aptgo_Click"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_aptnew" Text="Add New" runat="server" Visible="false" OnClick="btn_addapt_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_trnasgo" Text="Go" runat="server" Visible="false" OnClick="btn_trnasgo_Click"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_transnew" Text="Add New" runat="server" Visible="false" OnClick="btn_transnew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_relgo" Text="Go" runat="server" Visible="false" OnClick="btn_relgo_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_relnew" Text="Add New" runat="server" Visible="false" OnClick="btn_relnew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_aprgo" Text="Go" runat="server" Visible="false" OnClick="btn_aprgo_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_aprnew" Text="Add New" runat="server" Visible="false" OnClick="btn_aprnew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_promgo" Text="Go" runat="server" Visible="false" OnClick="btn_promgo_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_promonew" Text="Add New" runat="server" Visible="false" OnClick="btn_promonew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_incrgo" Text="Go" runat="server" Visible="false" OnClick="btn_incrgo_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_incrnew" Text="Add New" runat="server" Visible="false" OnClick="btn_incrnew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_loango" Text="Go" runat="server" Visible="false" OnClick="btn_loango_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_loannew" Text="Add New" runat="server" Visible="false" OnClick="btn_loannew_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                </td>
                            </tr>
                        </table>
                        <fieldset id="reldate" runat="server" visible="false" style="background-color: #ffccff; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: -700px; width: 200px;">
                            <asp:CheckBox ID="cb_relieve" runat="server" AutoPostBack="true" Font-Bold="true"
                                Font-Names="Book Antiqua" Style="margin-left: 0px;" Font-Size="Medium" Text="Relieve" />
                            <asp:CheckBox ID="cb_disc" runat="server" AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua"
                                Style="margin-left: 0px;" Font-Size="Medium" Text="Discontinue" />
                        </fieldset>
                    </center>
                </fieldset>
            </center>
            <center>
                <fieldset id="loan" runat="server" visible="false" style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: 0px; width: 924px;">
                    <table style="margin-left: -56px; margin-top: -10px;">
                        <tr>
                            <td>
                                <fieldset style="height: auto; width: auto; border-radius: 10px; background-color: #ffccff;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_loan" Text="Loan" runat="server" GroupName="l" Checked="true"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_policy" Text="Policy" runat="server" GroupName="l" Font-Names="Book Antiqua"
                                                    Font-Bold="true" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: auto; width: auto; border-radius: 10px; background-color: #ffccff;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_bank" Text="Bank Loan" runat="server" GroupName="bk" Checked="true"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_college" Text="College Loan" runat="server" GroupName="bk"
                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stfscode" runat="server" Text="Staff Code" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_scode" runat="server" MaxLength="15" CssClass="textbox txtheight2"
                                    Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_sname" runat="server" MaxLength="15" CssClass="textbox txtheight2 txtcapitalize"
                                    Style="width: 150px; font-weight: bold; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    <table style="margin-left: -520px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_policyno" runat="server" Text="Policy No" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_policyno" runat="server" MaxLength="15" CssClass="textbox txtheight2"
                                    Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetPolicyNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_policyno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_status" runat="server" Text="Status" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_status" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True">
                                            <asp:ListItem Value="1">All</asp:ListItem>
                                            <asp:ListItem Value="2">Active</asp:ListItem>
                                            <asp:ListItem Value="3">Closed</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>


                        </tr>

                    </table>
                </fieldset>
            </center>
            <br />
            <center>
                <div>
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="950px" Style="margin-top: -0.1%;">
                            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                </div>
                <br />
                <div>
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="950px">
                        <div id="divcolumn" runat="server" style="height: auto; width: 924px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                            Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
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
                                        <asp:ListBox ID="lstcolorder" runat="server" Visible="false"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </center>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <center>
                <div>
                    <center>
                        <asp:Label ID="lbl_err" runat="server" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                    </center>
                    <br />
                    <center>
                        <table id="tbllabel" runat="server" visible="false">
                            <tr>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblapplied" runat="server" Font-Bold="true" ForeColor="#0CA6CA" Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblselected" runat="server" Font-Bold="true" ForeColor="DarkBlue"
                                        Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblwaiting" runat="server" Font-Bold="true" ForeColor="DarkViolet"
                                        Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblrejected" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblappointed" runat="server" Font-Bold="true" ForeColor="DarkGreen"
                                        Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblrelieved" runat="server" Font-Bold="true" ForeColor="DarkRed" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table id="tbllabel1" runat="server" visible="false">
                            <tr>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblsel" runat="server" Font-Bold="true" ForeColor="DarkBlue" Text=""></asp:Label>
                                </td>
                                <td style="padding-left: 30px;">
                                    <asp:Label ID="lblwait" runat="server" Font-Bold="true" ForeColor="DarkViolet" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <farpoint:fpspread id="Fpspread4" runat="server" overflow="true" bordercolor="Black"
                            borderstyle="Solid" borderwidth="1px" oncellclick="Cell4_Click" onprerender="Fpspread4_OnPreRender"
                            visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread5" runat="server" onprerender="FpSpread5_OnPreRender"
                            oncellclick="Cell5_Click" overflow="true" bordercolor="Black" borderstyle="Solid"
                            borderwidth="1px" visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread6" runat="server" overflow="true" bordercolor="Black"
                            borderstyle="Solid" borderwidth="1px" oncellclick="Cell6_Click" onprerender="Fpspread6_OnPreRender"
                            visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread7" runat="server" overflow="true" bordercolor="Black"
                            borderstyle="Solid" borderwidth="1px" oncellclick="Cell_Click" onprerender="Fpspread7_OnPreRender"
                            visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread9" runat="server" onprerender="Fpspread9_render" oncellclick="Cell1_Click"
                            overflow="true" bordercolor="Black" borderstyle="Solid" borderwidth="1px" visible="false"
                            showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread10" runat="server" onprerender="Fpspread10_render"
                            oncellclick="Cell10_Click" overflow="true" bordercolor="Black" borderstyle="Solid"
                            borderwidth="1px" visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread14" runat="server" overflow="true" bordercolor="Black"
                            borderstyle="Solid" oncellclick="Cell14_Click" onprerender="Fpspread14_OnPreRender"
                            borderwidth="1px" visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread15" runat="server" onprerender="Fpspread15_render"
                            oncellclick="Cell15_Click" overflow="true" bordercolor="Black" borderstyle="Solid"
                            borderwidth="1px" visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread16" runat="server" oncellclick="Cell16_Click" onprerender="Fpspread16_OnPreRender"
                            overflow="true" bordercolor="Black" borderstyle="Solid" borderwidth="1px" visible="false"
                            showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                    <center>
                        <farpoint:fpspread id="Fpspread17" runat="server" overflow="true" bordercolor="Black"
                            oncellclick="Cell17_Click" onprerender="Fpspread17_OnPreRender" borderstyle="Solid"
                            borderwidth="1px" visible="false" showheaderselection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </center>
                </div>
            </center>
            <br />
            <br />
            <div id="rptprint" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txt_excelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <div id="rptprint1" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname1" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname1" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel1" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel1_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_excelname1"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster1" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster1_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
            </div>
            <div id="rptprint10" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation10" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname10" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname10" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel10" runat="server" Text="Export To Excel" Width="127px"
                    CssClass="textbox btn2" OnClick="btn_excel10_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_excelname10"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster10" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster10_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster2" Visible="false" />
            </div>
            <div id="rptprint5" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation5" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname5" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname5" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel5" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel5_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_excelname5"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster5" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster5_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster3" Visible="false" />
            </div>
            <div id="rptprint6" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation6" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname6" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname6" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel6" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel6_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_excelname6"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster6" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster6_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster4" Visible="false" />
            </div>
            <div id="rptprint9" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation9" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname9" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname9" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel9" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel9_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_excelname9"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster9" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster9_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster5" Visible="false" />
            </div>
            <div id="rptprint14" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation14" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname14" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname14" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel14" runat="server" Text="Export To Excel" Width="127px"
                    CssClass="textbox btn2" OnClick="btn_excel14_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender100" runat="server" TargetControlID="txt_excelname14"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster14" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster14_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster6" Visible="false" />
            </div>
            <div id="rptprint15" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation15" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname15" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname15" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel15" runat="server" Text="Export To Excel" Width="127px"
                    CssClass="textbox btn2" OnClick="btn_excel15_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender101" runat="server" TargetControlID="txt_excelname15"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster15" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster15_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster7" Visible="false" />
            </div>
            <div id="rptprint16" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation16" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname16" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname16" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel16" runat="server" Text="Export To Excel" Width="127px"
                    CssClass="textbox btn2" OnClick="btn_excel16_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender102" runat="server" TargetControlID="txt_excelname16"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster16" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster16_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster8" Visible="false" />
            </div>
            <div id="rptprint17" runat="server" visible="false" style="font-weight: bold;">
                <asp:Label ID="lbl_validation17" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label><br />
                <asp:Label ID="lbl_rptname17" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                <asp:TextBox ID="txt_excelname17" runat="server" Width="180px" onkeypress="display1()"
                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btn_excel17" runat="server" Text="Export To Excel" Width="127px"
                    CssClass="textbox btn2" OnClick="btn_excel17_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender103" runat="server" TargetControlID="txt_excelname17"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_printmaster17" runat="server" Text="Print" CssClass="textbox textbox1"
                    OnClick="btn_printmaster17_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                <Insproplus:printmaster runat="server" ID="Printmaster9" Visible="false" />
            </div>
        </center>
        <center>
            <div id="div_Add" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../images/close.png" Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 432px;"
                    OnClick="ImageButton6_Click" />
                <br />
                <center>
                    <div id="Div41" runat="server" class="sty2" style="background-color: White; height: 630px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="header" runat="server" Text="" ForeColor="Indigo" Style="font-weight: bold; font-family: book antiqua; font-size: large;"></asp:Label>
                            <fieldset id="Fieldset1" runat="server" style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: 110px; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: 0px; width: 824px;">
                                <table id="Table1" runat="server" style="margin-left: -35px;">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label2" runat="server" Text="College Name : " Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_clg" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="310px" OnSelectedIndexChanged="ddl_clg_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_dpt" runat="server" Text="Department : " Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_popdept" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                        <asp:CheckBox ID="cb_popdept" runat="server" Text="Select All" OnCheckedChanged="cb_popdept_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_popdept" runat="server" OnSelectedIndexChanged="cbl_popdept_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_popdept"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_dsg" runat="server" Text="Designation :" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_popdesig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                        <asp:CheckBox ID="cb_popdesig" runat="server" Text="Select All" OnCheckedChanged="cb_popdesig_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_popdesig" runat="server" OnSelectedIndexChanged="cbl_popdesig_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_popdesig"
                                                        PopupControlID="Panel2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_stf" runat="server" Text="Staff Type :" Style="font-weight: bold; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_popstype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; margin-left: 0px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel3" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; width: 175px; height: 200px;">
                                                        <asp:CheckBox ID="cb_popstype" runat="server" Text="Select All" OnCheckedChanged="cb_popstype_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_popstype" runat="server" OnSelectedIndexChanged="cbl_popstype_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_popstype"
                                                        PopupControlID="Panel3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsug" runat="server" Text="Status" Style="font-weight: bold; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updsug" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtstatus" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; margin-left: 0px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlstatus" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; width: 175px; height: 200px;">
                                                        <asp:CheckBox ID="cbstatus" runat="server" Text="Select All" OnCheckedChanged="cbstatus_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cblstatus" runat="server" OnSelectedIndexChanged="cblstatus_CheckedChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtstatus"
                                                        PopupControlID="pnlstatus" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_popgo" Text="Go" runat="server" OnClick="btn_popgo_OnClick" Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                <center>
                                    <asp:Label ID="lbl_alert" runat="server" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                                </center>
                                <asp:Button ID="btn_transfer1" Text="Transfer" runat="server" OnClick="btn_transfer1_OnClick"
                                    Style="font-weight: bold; margin-left: 725px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;"
                                    Visible="false" />
                                <asp:Button ID="btn_relieve1" Text="Relieve" runat="server" OnClick="btn_relieve1_OnClick"
                                    Style="font-weight: bold; font-family: book antiqua; margin-left: 725px; font-size: medium; background-color: #6699ee; border-radius: 6px;"
                                    Visible="false" />
                                <asp:Button ID="btn_appraisal1" Text="Appraisal" runat="server" OnClick="btn_appraisal1_OnClick"
                                    Style="font-weight: bold; font-family: book antiqua; margin-left: 725px; font-size: medium; background-color: #6699ee; border-radius: 6px;"
                                    Visible="false" />
                                <asp:Button ID="btn_promotion1" Text="Promotion" runat="server" OnClick="btn_promotion1_OnClick"
                                    Style="font-weight: bold; font-family: book antiqua; margin-left: 725px; font-size: medium; background-color: #6699ee; border-radius: 6px;"
                                    Visible="false" />
                                <asp:Button ID="btn_increment1" Text="Increment" runat="server" OnClick="btn_increment1_OnClick"
                                    Style="font-weight: bold; font-family: book antiqua; margin-left: 725px; font-size: medium; background-color: #6699ee; border-radius: 6px;"
                                    Visible="false" />
                                <center>
                                    <farpoint:fpspread id="Fpspread1" runat="server" overflow="true" bordercolor="Black"
                                        borderstyle="Solid" borderwidth="1px" width="820px" height="350px" class="spreadborder"
                                        visible="false" showheaderselection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </farpoint:fpspread>
                                </center>
                            </fieldset>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="Loan_add" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: absolute; width: 100%; z-index: 1000; height: 1000px;">
                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../images/close.png" Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 432px;"
                    OnClick="ImageButton7_Click" />
                <br />
                <center>
                    <div id="Div5" runat="server" class="sty2" style="background-color: White; height: 700px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <fieldset style="height: auto; width: 250px; background-color: #B0C4DE; border-radius: 10px;">
                                <asp:Label ID="loan_header" runat="server" Text="" ForeColor="Indigo" Style="font-weight: bold; font-family: book antiqua; font-size: large;"></asp:Label>
                            </fieldset>
                            <br />
                        </center>
                        <table style="margin-left: 20px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_popscode" runat="server" Text="Staff Code" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_popscode" runat="server" AutoPostBack="true" OnTextChanged="txt_popscode_OnTextChanged"
                                        CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_popscode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <asp:Button ID="btn_scode" Text="?" runat="server" OnClick="btn_scode_OnClick" Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_popsname" runat="server" Text="Staff Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_popsname" runat="server" CssClass="textbox txtheight2 txtcapitalize"
                                        Style="width: 250px; font-weight: bold; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_loantype" runat="server" Text="Type" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <fieldset style="height: 10px; width: auto; border-radius: 10px;">
                                        <table style="margin-top: -5px;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rdb_poploan" Text="Loan" runat="server" AutoPostBack="true"
                                                        OnCheckedChanged="rdb_poploan_OnCheckedChanged" GroupName="lc" Checked="true"
                                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdb_popppolicy" Text="Policy" runat="server" AutoPostBack="true"
                                                        OnCheckedChanged="rdb_popppolicy_OnCheckedChanged" GroupName="lc" Font-Names="Book Antiqua"
                                                        Font-Bold="true" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_loanfrom" runat="server" Text="Loan From" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <fieldset style="height: 10px; width: auto; border-radius: 10px;">
                                        <table style="margin-top: -5px;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rdb_popbank" Text="Bank Loan" runat="server" GroupName="cd"
                                                        Checked="true" AutoPostBack="true" OnCheckedChanged="rdb_popbank_change" Font-Bold="true"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdb_popclg" Text="College Loan" runat="server" GroupName="cd"
                                                        AutoPostBack="true" OnCheckedChanged="rdb_popclg_change" Font-Names="Book Antiqua"
                                                        Font-Bold="true" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_loandate" runat="server" Text="Loan Date" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_loandate" runat="server" AutoPostBack="true" OnTextChanged="txt_loandate_Change"
                                        CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txt_loandate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                        <table style="margin-left: -200px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_loanname" runat="server" Text="Loan Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_loanname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_loanname_OnSelectedIndexChanged"
                                        CssClass="textbox ddlstyle ddlheight3" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_lamt" runat="server" Text="Loan Amount" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_lamt" runat="server" onblur="chkdoubleamnt(this);" onkeyup="chkdoubleamnt(this);"
                                        CssClass="textbox txtheight2" onchange="return chkdedamnt()" MaxLength="15" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender73" runat="server" TargetControlID="txt_lamt"
                                        FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <fieldset id="fld_policy" runat="server" visible="false" style="height: auto; width: 700px; margin-left: -70px;">
                            <table style="margin-left: -300px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_companyname" runat="server" Text="Company Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_companyname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_companyname_OnSelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_policyname" runat="server" Text="Policy Name" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_policyname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_policyname_OnSelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: -170px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pno" runat="server" Text="Policy No" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pno" runat="server" MaxLength="15" CssClass="textbox txtheight2"
                                            Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_pno"
                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pdate" runat="server" Text="Policy Date" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pdate" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_pdate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_polamt" runat="server" Text="Policy Amount" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_polamt" runat="server" onblur="chkdoubleamnt(this);" onkeyup="chkdoubleamnt(this);"
                                            MaxLength="15" CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_polamt"
                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_prmamt" runat="server" Text="Premium Amount" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_prmamt" runat="server" onblur="chkdoubleamnt(this);" onkeyup="chkdoubleamnt(this);"
                                            MaxLength="15" CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_prmamt"
                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset id="fld_bank" runat="server" visible="false" style="height: auto; width: 390px; margin-left: -381px; border-radius: 10px;">
                            <table style="margin-left: 0px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbankfldbankname" runat="server" Text="Bank Name" Style="font-weight: bold; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_fldbankname" runat="server" CssClass="textbox1 ddlheight5"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_fldbankname_change">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbankfldbranchname" runat="server" Text="Branch Name" Style="font-weight: bold; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_fldbranchname" runat="server" CssClass="textbox1 ddlheight5"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_fldbranchname_change">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset id="fld_loan" runat="server" visible="false" style="height: auto; width: 350px; margin-left: -420px; border-radius: 10px;">
                            <table style="margin-left: 0px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_interest" runat="server" Text="Is Interest" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"
                                            AutoPostBack="true" OnCheckedChanged="cb_interest_Changed" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_percent" runat="server" Text="Percentage" Style="font-weight: bold; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_percentage" runat="server" onkeyup="return percent()" onblur="return percent()"
                                            MaxLength="3" Enabled="false" CssClass="textbox txtheight2" Style="font-weight: bold; width: 80px; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_percentage"
                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_tenure" runat="server" Text="Loan Tenure" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_tenure" runat="server" MaxLength="3" Enabled="false" onchange="return chkdedamnt()"
                                            CssClass="textbox txtheight2" Style="font-weight: bold; width: 80px; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender108" runat="server" TargetControlID="txt_tenure"
                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_mnths" runat="server" Text="Months" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                        <asp:Button ID="btnshowcal" runat="server" Text="Show" OnClick="btnshowcal_click"
                                            Font-Bold="true" Font-Names="Book Antiqua" CssClass="textbox textbox1 btn2" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <fieldset style="height: 20px; width: 400px; border-radius: 10px; margin-left: -370px;">
                            <table style="margin-top: -5px; margin-left: -10px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_active" runat="server" Text="Is Active" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"
                                            AutoPostBack="true" OnCheckedChanged="cb_active_Change" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_closed" runat="server" Text="Is Closed" OnCheckedChanged="cb_closed_OnCheckedChanged"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_closeddate" runat="server" Text="Closed Date" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_closeddate" runat="server" AutoPostBack="true" OnTextChanged="txt_closeddate_Change"
                                            Enabled="false" CssClass="textbox txtheight2" Style="font-weight: bold; width: 110px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_closeddate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <asp:CheckBox ID="cbdedfrmsal" runat="server" Text="Deduct From Salary" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -618px;"
                            AutoPostBack="true"
                            OnCheckedChanged="cbdedfrmsal_Change" />
                        <br />
                        <fieldset id="dedfld" runat="server" visible="false" style="border-radius: 10px; width: 500px;">
                            <table>
                                <tr>
                                    <td>Deduction Name :
                                    </td>
                                    <td>Ded Amt Per Month
                                    </td>
                                    <td>Deduction From :
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddl_dedname" runat="server" CssClass="textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_dedamt" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_dedfrom" runat="server" CssClass="textbox1 ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <table style="margin-left: -305px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_rmks" runat="server" Text="Remarks" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rmks" runat="server" TextMode="MultiLine" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 400px; margin-left: 10px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_loansave" Text="Save" runat="server" Visible="false" OnClick="btn_loansave_Onclick"
                                            Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                        <asp:Button ID="btn_loanupdate" Text="Update" runat="server" Visible="false" OnClick="btn_loanupdate_OnClick"
                                            Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                        <asp:Button ID="btn_loandelete" Text="Delete" runat="server" Visible="false" OnClick="btn_loandelete_OnClick"
                                            Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                        <asp:Button ID="btn_loanexit" Text="Exit" runat="server" OnClick="btn_loanexit_OnClick"
                                            Style="font-weight: bold; margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="poploancalc" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="imgpoploancalc" runat="server" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 432px;"
                    OnClick="imgpoploancalc_Click" />
                <br />
                <center>
                    <div id="divloanpop" runat="server" class="sty2" style="background-color: White; height: 550px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lblloandet" runat="server" Text="Staff Loan With Interest" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Larger" ForeColor="Green"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <fieldset id="fldloandet" runat="server" style="width: 370px; height: 80px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmonthemi" runat="server" Text="Monthly PayMent (EMI)    " Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="DarkBlue"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblemi" runat="server" Text="" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Brown"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltotintpay" runat="server" Text="Total Interest Payable        "
                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="DarkBlue"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblinterest" runat="server" Text="" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Brown"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltotpay" runat="server" Text="Total Payment (Principal+Interest)   "
                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="DarkBlue"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltotpayamnt" runat="server" Text="" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Brown"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </center>
                        <br />
                        <center>
                            <farpoint:fpspread id="Fpspreadloan" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="760px" height="300px" class="spreadborder"
                                showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                            <br />
                            <asp:Button ID="btnintloan_Hide" runat="server" Text="Hide" CssClass="textbox textbox1 btn2"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnintloan_Hide_click" />
                            <br />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="Internal" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                <center>
                    <div id="Div6" runat="server" class="table" style="background-color: White; height: 530px; width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 50px; border-radius: 10px;">
                        <center>
                            <br />
                            <center>
                                <br />
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblpoploandept" runat="server" Text="Department" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddeptloan" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtpoploandept" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; margin-left: 0px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="poppnldept" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; width: 250px; height: 200px;">
                                                        <asp:CheckBox ID="cbpoploandept" runat="server" Text="Select All" OnCheckedChanged="cbpoploandept_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cblpoploandept" runat="server" OnSelectedIndexChanged="cblpoploandept_CheckedChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtpoploandept"
                                                        PopupControlID="poppnldept" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblpoploandesig" runat="server" Text="Designation" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updpoploandes" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtpoploandesig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; margin-left: 0px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="poppnldes" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; width: 250px; height: 200px;">
                                                        <asp:CheckBox ID="cbpoploandes" runat="server" Text="Select All" OnCheckedChanged="cbpoploandes_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cblpoploandes" runat="server" OnSelectedIndexChanged="cblpoploandes_CheckedChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtpoploandesig"
                                                        PopupControlID="poppnldes" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <%--Staff Category added by Poomalar 13.10.17--%>
                                        <td>
                                            <asp:Label ID="lblpoploancateg" runat="server" Text="Staff Category" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upstaffcat" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_staffcat" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnl_staffcat" runat="server" CssClass="multxtpanel" Height="200px"
                                                        Width="200px">
                                                        <asp:CheckBox ID="cb_staffcat" runat="server" AutoPostBack="true" OnCheckedChanged="cb_staffcat_CheckedChanged"
                                                            Text="Select All" />
                                                        <asp:CheckBoxList ID="cbl_staffcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffcat_selectedchanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" PopupControlID="pnl_staffcat"
                                                        TargetControlID="txt_staffcat" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_searchby" runat="server" Text="Search By" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_searchby" runat="server" Font-Bold="true" OnSelectedIndexChanged="ddl_searchby_OnSelectedIndexChanged"
                                                CssClass="textbox txtheight5" Style="height: 30px;" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="150px" AutoPostBack="True">
                                                <asp:ListItem Selected="True" Value="0" Text="All"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Staff Code"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Staff Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txt_searchscode" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchscode"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>

                                            <%--added by Poomalar 13.10.17--%>
                                            <asp:Button ID="Button1" Text=" Go " runat="server" OnClick="btn_intgo_OnClick"
                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_searchsname" runat="server" Visible="false" CssClass="textbox txtheight2 txtcapitalize"
                                                Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchsname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <%--commanded by Poomalar 13.10.17--%>
                                        <%--<td>                                       
                                            <asp:Button ID="btn_intgo" Text=" Go " runat="server" OnClick="btn_intgo_OnClick"
                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />                                                
                                        </td>--%>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="int_alert" runat="server" Text="" Visible="false" Font-Bold="true"
                                    Font-Names="Book Antiqua" Style="color: Red;" Font-Size="Medium"></asp:Label></center>
                            <br />
                            <center>
                                <farpoint:fpspread id="Fpspread8" runat="server" overflow="true" bordercolor="Black"
                                    borderstyle="Solid" borderwidth="1px" width="850px" height="300px" class="spreadborder"
                                    showheaderselection="false" onbuttoncommand="Fpspread8_Command" style="border-radius: 10px; margin-left: 1px;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </farpoint:fpspread>
                            </center>
                            <br />
                            <div>
                                <center>
                                    <asp:Button ID="btn_intAdd" Text=" Add " OnClick="btn_intAdd_OnClick" runat="server"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    <asp:Button ID="btn_intexit" Text=" Exit " runat="server" OnClick="btn_intexit_OnClick"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="div_Addapt" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: absolute; width: 100%; z-index: 1000; height: 1000em;">
                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../images/close.png" Style="height: 30px; width: 30px; position: absolute; margin-top: 242px; margin-left: 500px;"
                    OnClick="ImageButton8_Click" />
                <span class="fontstyleheader" style="color: indigo; position: absolute; margin-top: 239px; margin-left: -165px;">Staff Appointment Entry</span>
                <center>
                    <div id="Div7" runat="server" class="sty2" style="background-color: White; height: 800px; width: 1050px; border: 5px solid #0CA6CA; margin-top: 242px; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <br />
                            <asp:Label ID="err" runat="server" Visible="false" Style="color: Red;" Height="38px"
                                Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_applno" runat="server" Text="Application No" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_applno" runat="server" MaxLength="15" OnTextChanged="txt_applno_OnTextChanged"
                                            CssClass="textbox txtheight2 " Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                        <asp:Button ID="btn_applno" Text="?" runat="server" OnClick="btn_applno_OnClick"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fname" runat="server" Text="First Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fname" runat="server" MaxLength="25" CssClass="textbox txtheight2 "
                                            Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_mname" runat="server" Text="Middle Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_mname" runat="server" MaxLength="20" CssClass="textbox txtheight2"
                                            Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_surname" runat="server" Text="Sur Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_surname" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: -205px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_aptscode" runat="server" Text="Staff Code" Style="margin-left: 5px;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_aptscode" runat="server" MaxLength="15" AutoPostBack="true"
                                            OnTextChanged="txt_aptscode_OnTextChanged" onkeypress="lblvis()" onblur="return get(this.value)"
                                            onfocus="return myFunction(this)" CssClass="textbox txtheight2" Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 29px;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="spnacr" style="color: Red;">*</span>
                                        <asp:Label ID="lblstfalert" runat="server" Visible="false" Text="" Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="lblstf" runat="server" Text="" Visible="false" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stream" runat="server" Text="Stream" Style="margin-left: 8px;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_streamplus" Text="+" runat="server" OnClick="btn_streamplus_OnClick"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Stream" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_streamminus" Text="-" runat="server" OnClick="btn_streamminus_OnClick"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_adate" runat="server" Text="Appointed Date" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_adate" runat="server" AutoPostBack="true" OnChange="return checkDate()"
                                            CssClass="textbox txtheight2" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender11" TargetControlID="txt_adate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                            <fieldset style="height: auto;">
                                <center>
                                    <fieldset style="height: 300px; margin-left: -710px; width: 275px;">
                                        <u>
                                            <asp:Label ID="header1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="color: indigo;">Applied Details</asp:Label>
                                        </u>
                                        <br />
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_papplied" runat="server" Text="Post Applied" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_papplied" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_popdept" runat="server" Text="Department" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_pdep" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_popstype" runat="server" Text="Staff Type" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_pstyp" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="cdet" runat="server" Text="Certificate Details" OnClick="cdet_OnClick"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="Exp" runat="server" Text="Experience Certificate" OnClick="Exp_OnClick"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td colspan="2">
                                                    <br />
                                                    <asp:LinkButton ID="lbStfDet" runat="server" Text="Staff's Children Details" OnClick="lbStfDet_OnClick"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_priority" runat="server" Text="Set Priority" align="center" Font-Bold="true" Font-Size="Medium"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_priority" runat="server" CssClass="textbox txtheight2"
                                                        Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                </td>


                                            </tr>
                                            <tr>
                                                <asp:CheckBox ID="cb_deptpriority" runat="server" />

                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset style="height: auto; margin-left: -55px; margin-top: -322px; width: 320px;">
                                        <u>
                                            <asp:Label ID="header2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="color: indigo;">Appointed Details</asp:Label>
                                        </u>
                                        <br />
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_pappointed" runat="server" Text="Post Appointed" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pappointed" runat="server" Font-Bold="true" Width="200px"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_appdept" runat="server" Text="Department" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_appdept" runat="server" Width="200px" Font-Bold="true"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_appstype" runat="server" Text="Staff Type" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_appstype" runat="server" Width="200px" Font-Bold="true"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" OnSelectedIndexChanged="ddl_appstype_Change"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="trStfType" runat="server" visible="false">
                                                <td></td>
                                                <td>
                                                    <asp:TextBox ID="txtStfType" runat="server" CssClass="textbox txtheight3" Width="200px" Font-Bold="true"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_appcat" runat="server" Text="Category" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_appcat" runat="server" Width="200px" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_appdoj" runat="server" Text="Date Of Join" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_appdoj" runat="server" AutoPostBack="true" OnChange="return checkDate()"
                                                        OnFocus="return myFunction(this)" CssClass="textbox txtheight2" Font-Bold="true"
                                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                                    </asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender22" TargetControlID="txt_appdoj" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_payscale" runat="server" Text="Pay Scale" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_payscale" OnSelectedIndexChanged="ddl_payscale_OnSelectedIndexChanged"
                                                        runat="server" Width="200px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_shift" runat="server" Text="Shift" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_shift" runat="server" Width="200px" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset style="margin-left: 526px; margin-top: -326px; width: 200px;">
                                        <legend style="color: indigo;">Nature</legend>
                                        <asp:RadioButton ID="rdb_ftime" runat="server" Text="Full Time" AutoPostBack="true"
                                            GroupName="na" Checked="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:RadioButton ID="rdb_ptime" runat="server" Text="Part Time" AutoPostBack="true"
                                            GroupName="na" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:CheckBox ID="cb_honour" runat="server" Text="Honoured Staff" OnCheckedChanged="cb_dept_CheckedChange"
                                            AutoPostBack="true" />
                                    </fieldset>
                                    <fieldset style="margin-left: 525px; margin-top: 0px; width: 200px;">
                                        <legend style="color: indigo;">Pay Type</legend>
                                        <asp:RadioButton ID="rdb_regular" runat="server" Text="Regular" AutoPostBack="true"
                                            GroupName="b" Checked="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:RadioButton ID="rdb_cons" runat="server" Text="Consolidate" AutoPostBack="true"
                                            GroupName="b" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </fieldset>
                                    <asp:Label ID="lbl_popstat" runat="server" Text="Status" Style="margin-left: 350px;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddl_popstat" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 569px; margin-top: -18px;"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="1">Temporary</asp:ListItem>
                                        <asp:ListItem Value="2">Permanent</asp:ListItem>
                                        <asp:ListItem Value="3">Probationary</asp:ListItem>
                                        <asp:ListItem Value="4">Visiting</asp:ListItem>
                                        <asp:ListItem Value="5">Casual</asp:ListItem>
                                        <asp:ListItem Value="6">Contract</asp:ListItem>
                                        <asp:ListItem Value="7">Trainee</asp:ListItem>
                                        <asp:ListItem Value="8">Regular/Approved</asp:ListItem>
                                    </asp:DropDownList>
                                    <fieldset style="margin-left: 525px; margin-top: 0px; width: 200px;">
                                        <legend style="color: indigo;">Permanent Period</legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_popfd" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_popfd" runat="server" AutoPostBack="true" OnTextChanged="txt_popfd_OnTextChanged"
                                                        CssClass="textbox txtheight2" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="txt_popfd" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_poptd" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_poptd" runat="server" AutoPostBack="true" OnTextChanged="txt_poptd_OnTextChanged"
                                                        Style="margin-left: 0px;" CssClass="textbox txtheight2" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender13" TargetControlID="txt_poptd" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <asp:CheckBox ID="chkmanuallop" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="ManualLOP" Style="margin-left: 406px; margin-top: 5px;" />
                                    <fieldset style="height: 116px; margin-left: 886px; margin-top: -276px; width: 100px;">
                                        <asp:Image ID="stf_img" runat="server" Width="100px" Height="128px" />
                                        <asp:Image ID="Image2" runat="server" Visible="false" Width="100px" Height="128px" />
                                    </fieldset>
                                    <asp:Button ID="btn_photoadd" Text="Add Photo" runat="server" OnClick="btn_photoadd_OnClick"
                                        Style="font-weight: bold; margin-left: 0px; margin-left: 890px; margin-top: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    <fieldset style="margin-left: 890px; margin-top: 6px; width: 100px;">
                                        <legend style="color: indigo;">Retirement Date</legend>
                                        <asp:TextBox ID="txt_retire" runat="server" Style="margin-left: 0px;" CssClass="textbox txtheight2"
                                            Width=" 100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender14" TargetControlID="txt_retire" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </fieldset>
                                </center>
                            </fieldset>
                            <table width="950px;" style="margin-left: -60px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_bankdet" runat="server" Text="Bank Details :" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="color: indigo;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="sign" runat="server" Text="Signature Settings" OnClick="sign_click"
                                            Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="license" runat="server" Text="License Photo" OnClick="licensePhoto_OnClick" Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_ftype" runat="server" Text="Faculty Type" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_fplus" Text="+" runat="server" OnClick="btn_fplus_OnClick" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_ftype" runat="server" Width="200px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_fminus" Text="-" runat="server" OnClick="btn_fminus_OnClick"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_incr" runat="server" Text="No Of Increments" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_incr" runat="server" MaxLength="2" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 30px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_incr"
                                            FilterType="Numbers" ValidChars="Numbers" />
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <table style="float: left;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_paymode" runat="server" Text="Payment Mode :" Font-Bold="true"
                                                Font-Size="Medium" Style="color: indigo;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_cash" runat="server" Text="Cash" AutoPostBack="true" Width=" 80px"
                                                GroupName="c" Checked="true" Font-Bold="true" OnCheckedChanged="rdb_cash_Clcik"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_cheque" runat="server" Text="Cheque" AutoPostBack="true"
                                                Width=" 80px" GroupName="c" Font-Bold="true" OnCheckedChanged="rdb_cheque_Clcik"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_credit" runat="server" Text="Credit" AutoPostBack="true"
                                                Width=" 80px" GroupName="c" Font-Bold="true" OnCheckedChanged="rdb_credit_Clcik"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="CreditTable" runat="server" visible="false" style="margin-left: 35px; float: left;">
                                    <tr>
                                        <td>
                                            <fieldset style="margin-left: 0px; width: 300px;">
                                                <asp:RadioButton ID="rdb_own" runat="server" Text="Own Account" AutoPostBack="true"
                                                    GroupName="d" Checked="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                <asp:RadioButton ID="rdb_nominne" runat="server" Text="Nominee Account" AutoPostBack="true"
                                                    GroupName="d" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ifsc" runat="server" Text="IFSC Code" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ifsc" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                                CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table id="CreditTable1" runat="server" visible="false" style="margin-left: -5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_acno" runat="server" Text="Account No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_acno" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                                CssClass="textbox txtheight1" Width=" 200px" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_acno"
                                                FilterType="Numbers" ValidChars="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_bankname" runat="server" Text="Bank Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_bnameplus" Text="+" runat="server" OnClick="btn_bnameplus_OnClick"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_bankname" runat="server" Width="200px" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_bnameminus" Text="-" runat="server" OnClick="btn_bnameminus_OnClick"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_branchname" runat="server" Text="Branch Name" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_branchplus" Text="+" runat="server" OnClick="btn_branchplus_OnClick"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_branchname" runat="server" Width="200px" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_branchminus" Text="-" runat="server" OnClick="btn_branchminus_OnClick"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbl_collBank" runat="server" Text="College Bank" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                            <asp:DropDownList ID="ddl_colbank" runat="server" Width="200px" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>
                            </fieldset>
                            <table style="margin-left: -35px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pfno" runat="server" Text="PF No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pfno" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_pfno"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_panno" runat="server" Text="PAN No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_panno" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_panno"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_licno" runat="server" Text="LIC No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_licno" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_licno"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_adharno" runat="server" Text="Aadhar No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_adharno" runat="server" MaxLength="14" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_adharno"
                                            FilterType="custom,Numbers" ValidChars="0123456789 " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_loanno" runat="server" Text="Loan No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_loanno" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_loanno"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_gpf" runat="server" Text="GPF No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_gpf" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_gpf"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_esi" runat="server" Text="ESI No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_esi" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_esi"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_uan" runat="server" Text="UAN No" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_uan" runat="server" MaxLength="15" Style="margin-left: 0px;"
                                            CssClass="textbox txtheight1" Width=" 150px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender109" runat="server" TargetControlID="txt_uan"
                                            FilterType="custom,Numbers,uppercaseletters,lowercaseletters" ValidChars="custom " />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6"></td>
                                    <td colspan="4">
                                        <asp:Button ID="btn_appoint" Text="Appoint" Visible="false" runat="server" OnClick="btn_appoint_OnClick"
                                            OnClientClick="return checkDate()" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_apporder" Text="Appointment Order" Visible="false" runat="server"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_update" Text="Update" Visible="false" OnClick="btn_update_OnClick"
                                            runat="server" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_addclose" Text="Close" runat="server" OnClick="btn_addclose_OnClick"
                                            Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <div id="Application" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                    <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="../images/close.png" Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 405px;"
                                        OnClick="ImageButton9_Click" />
                                    <br />
                                    <center>
                                        <div id="Div8" runat="server" class="sty2" style="background-color: White; height: 550px; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <fieldset style="width: 280px; height: 20px; background-color: #ffccff; margin-left: 0px; border-radius: 10px; border-color: #6699ee;">
                                                <span class="fontstyleheader" style="color: indigo;">Applied Staff List</span>
                                            </fieldset>
                                            <br />
                                            <table style="margin-left: -250px;">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_applied" runat="server" Text="Applied Staff" OnCheckedChanged="rdb_applied_OnCheckedChanged"
                                                            AutoPostBack="true" GroupName="e" Checked="true" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_selected" runat="server" Text="Selected Staff" OnCheckedChanged="rdb_selected_OnCheckedChanged"
                                                            AutoPostBack="true" GroupName="e" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_relievd" runat="server" Text="Relieved Staff" AutoPostBack="true"
                                                            OnCheckedChanged="rdb_relievd_OnCheckedChanged" GroupName="e" Font-Bold="true"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_discontinued" runat="server" Text="Discontniued Staff" AutoPostBack="true"
                                                            OnCheckedChanged="rdb_discontinued_OnCheckedChanged" GroupName="e" Font-Bold="true"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <center>
                                                <farpoint:fpspread id="Fpspread11" runat="server" overflow="true" bordercolor="Black"
                                                    borderstyle="Solid" borderwidth="1px" width="800px" height="350px" class="spreadborder"
                                                    showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Blue">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </farpoint:fpspread>
                                                <br />
                                                <asp:Button ID="btn_ok" Text="OK" runat="server" OnClick="btn_ok_OnClick" Visible="false"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="DetailCertificate" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                    <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="../images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 405px;"
                                        OnClick="ImageButton10_Click" />
                                    <br />
                                    <center>
                                        <div id="Div9" runat="server" class="sty2" style="background-color: White; height: 550px; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <center>
                                                <fieldset style="width: 225px; height: 20px; background-color: #ffccff; margin-left: 0px; border-radius: 10px; border-color: #6699ee;">
                                                    <span class="fontstyleheader1" style="color: indigo;">Certificate Details</span>
                                                </fieldset>
                                                <br />
                                                <asp:Button ID="btn_cdetnew" Text="Add New" runat="server" OnClick="btn_cdetnew_OnClick"
                                                    Visible="false" Style="font-weight: bold; margin-left: 650px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <farpoint:fpspread id="Fpspread12" runat="server" overflow="true" bordercolor="Black"
                                                    borderstyle="Solid" borderwidth="1px" width="800px" height="350px" class="spreadborder"
                                                    showheaderselection="false" oncellclick="Fpspread12_Click" onprerender="Fpspread12_render"
                                                    style="border-radius: 10px; margin-left: 1px;">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </farpoint:fpspread>
                                                <br />
                                                <asp:Button ID="btn_detcertificate" Text="OK" runat="server" Visible="false" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="ExpCertificate" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                    <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="../images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 405px;"
                                        OnClick="ImageButton11_Click" />
                                    <br />
                                    <center>
                                        <div id="Div10" runat="server" class="sty2" style="background-color: White; height: 550px; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <center>
                                                <fieldset style="width: 335px; height: 20px; background-color: #ffccff; margin-left: 0px; border-radius: 10px; border-color: #6699ee;">
                                                    <span class="fontstyleheader1" style="color: indigo;">Experience Certificate Details</span>
                                                </fieldset>
                                                <br />
                                                <asp:Button ID="btn_newexp" Text="Add New" runat="server" OnClick="btn_newexp_OnClick"
                                                    Visible="false" Style="font-weight: bold; margin-left: 650px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <farpoint:fpspread id="Fpspread13" runat="server" overflow="true" bordercolor="Black"
                                                    borderstyle="Solid" borderwidth="1px" width="760px" height="350px" class="spreadborder"
                                                    showheaderselection="false" onprerender="Fpspread13_render" oncellclick="Fpspread13_Click"
                                                    style="border-radius: 10px; margin-left: 1px;">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </farpoint:fpspread>
                                                <br />
                                                <asp:Button ID="Button2" Text="OK" runat="server" Visible="false" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="divStfChild" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                    <asp:ImageButton ID="ImageButton15" runat="server" ImageUrl="../images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 405px;"
                                        OnClick="ImageButton15_Click" />
                                    <br />
                                    <center>
                                        <div id="myDivStf" runat="server" class="sty2" style="background-color: White; height: auto; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <span class="fontstyleheader" style="color: Green;">Add Staff's Children</span>
                                            <br />
                                            <table class="maintablestyle">
                                                <tr>
                                                    <td>College
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlclgStfChild" runat="server" CssClass="textbox1 ddlheight4" OnSelectedIndexChanged="ddlclgStfChild_Change" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td>Batch Year
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBatchStfChild" runat="server" CssClass="textbox1 ddlheight"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblstfchild_degree" Text="Degree" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtstfchild_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                    Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Gray" BorderStyle="Solid"
                                                                    BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                                    <asp:CheckBox ID="cbstfchild_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cbstfchild_degree_checkedchange" />
                                                                    <asp:CheckBoxList ID="cblstfchild_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstfchild_degree_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtstfchild_degree"
                                                                    PopupControlID="Panel4" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblstfchild_branch" Text="Branch" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upp5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtstfchild_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                                    Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="Panel9" runat="server" BackColor="White" BorderColor="Gray" BorderStyle="Solid"
                                                                    BorderWidth="2px" CssClass="multxtpanel" Width="200px" Height="200px" Style="position: absolute;">
                                                                    <asp:CheckBox ID="cbstfchild_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cbstfchild_branch_checkedchange" />
                                                                    <asp:CheckBoxList ID="cblstfchild_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstfchild_branch_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txtstfchild_branch"
                                                                    PopupControlID="Panel9" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStfChildAdmNo" Text="Admission No" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtStfChildAdmNo" runat="server" MaxLength="100" AutoPostBack="true"
                                                            CssClass="textbox txtheight2 txtcapitalize"
                                                            Style="width: 150px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetStudAdmNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtStfChildAdmNo"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="txtsearchpan">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:Button ID="btnStfChildGo" runat="server" Text="GO" CssClass="textbox1 btn1" OnClick="btnStfChildGo_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Label ID="lblMainStfErr" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                            <br />
                                            <asp:Button ID="BtnStudAdd" runat="server" Visible="false" CssClass="textbox1 btn2" Text="Add Student" Style="position: relative; left: 307px;" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Width="150px" BackColor="LightGreen" OnClick="BtnStudAdd_Click" />
                                            <farpoint:fpspread id="Fpspread22" runat="server" visible="false" overflow="true" bordercolor="Black"
                                                borderstyle="Solid" borderwidth="1px" width="760px" height="350px" class="spreadborder" onbuttoncommand="Fpspread22_Command"
                                                showheaderselection="false"
                                                style="border-radius: 10px; margin-left: 1px;">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </farpoint:fpspread>
                                            <br />
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <div id="Exper" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                <fieldset class="sty2" style="background-color: White; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top: 150px; width: 540px; height: 150px;">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_cname" runat="server" Text="Concern Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_cname" runat="server" Width="150px" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_cname_Change">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="certdate" runat="server" Text="Date of Issue" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_certdate" runat="server" Style="margin-left: 0px;" CssClass="textbox txtheight2"
                                                    Width=" 100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender15" TargetControlID="txt_certdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:CheckBox ID="cb_recieved" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Text="Received" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_original" runat="server" Text="Original" Checked="true"
                                                    GroupName="f" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_duplicate" runat="server" Text="Duplicate" GroupName="f"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_certok" Text="OK" runat="server" OnClick="btn_certok_Click" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <asp:Button ID="btn_certdel" Text="Delete" Visible="false" runat="server" OnClick="btn_certdel_Click"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <asp:Button ID="btn_certclose" Text="Close" OnClick="btn_certclose_OnClick" runat="server"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div id="cert" runat="server" visible="false" style="height: 1000px; z-index: 1000; width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                <fieldset class="sty2" style="background-color: White; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top: 150px; width: 520px;">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_deg" runat="server" Text="Degree" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_deg" runat="server" OnSelectedIndexChanged="ddl_deg_change"
                                                    Width="150px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_certname" runat="server" Text="Certificate Name" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_certname" runat="server" OnSelectedIndexChanged="ddl_certname_change"
                                                    Width="150px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_certno" runat="server" Text="Certificate No" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_certno" runat="server" Style="margin-left: 0px;" CssClass="textbox txtheight2"
                                                    Width=" 100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filtercertno" runat="server" FilterType="Custom,Numbers"
                                                    TargetControlID="txt_certno">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_expdate" runat="server" Text="Date of Issue" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_expdate" runat="server" Style="margin-left: 0px;" CssClass="textbox txtheight2"
                                                    Width=" 100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender116" TargetControlID="txt_expdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadioButton2" runat="server" Text="Original" AutoPostBack="true"
                                                    Checked="true" GroupName="g" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButton3" runat="server" Text="Duplicate" AutoPostBack="true"
                                                    GroupName="g" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_expok" Text="OK" runat="server" OnClick="btn_expok_click" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <asp:Button ID="btn_expdel" Text="Delete" runat="server" Visible="false" OnClick="btn_expdel_Click"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                <asp:Button ID="btn_expclose" Text="Close" OnClick="btn_expclose_OnClick" runat="server"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <center>
                                <div id="Plusapt" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <div id="Div112" runat="server" class="table" style="background-color: White; height: 150px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                                            <center>
                                                <br />
                                                <table style="line-height: 30px">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="headerapt" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_addstream" runat="server" MaxLength="25" CssClass="textbox txtheight2"
                                                                Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="line-height: 35px">
                                                            <asp:Button ID="btn_plusAdd" Text=" Add " Visible="false" runat="server" OnClick="btn_plusAdd_OnClick"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                            <asp:Button ID="btn_ftypeadd" Text=" Add " Visible="false" OnClick="btn_ftypeadd_OnClick"
                                                                runat="server" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                            <asp:Button ID="btn_bnameadd" Text=" Add " Visible="false" runat="server" OnClick="btn_bnameadd_OnClick"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                            <asp:Button ID="btn_branchadd" Text=" Add " Visible="false" runat="server" OnClick="btn_branchadd_OnClick"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                            <asp:Button ID="btn_Plusexit" Text=" Exit " runat="server" OnClick="btn_Plusexit_OnClick"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="Transfer" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 395px;"
                    OnClick="ImageButton1_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 830px; height: 400px;"
                    align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 325px; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Transfer</span>
                        </center>
                        <br />
                        <asp:Label ID="lbl_transdate" runat="server" Text="Transfer Date" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 33px;"></asp:Label>
                        <asp:TextBox ID="txt_transdate" runat="server" AutoPostBack="true" OnTextChanged="txt_transdate_Change"
                            CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 30px;"></asp:TextBox>
                        <asp:CalendarExtender ID="caldatestart" TargetControlID="txt_transdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <span style="color: Red;">*</span>
                        <asp:CheckBox ID="cb_scode" runat="server" AutoPostBack="true" Text="New StaffCode"
                            OnCheckedChanged="cb_scode_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 80px;" />
                        <asp:TextBox ID="txt_transscode" runat="server" MaxLength="15" AutoPostBack="true"
                            OnTextChanged="txt_transscode_OnTextChanged" Visible="false" CssClass="textbox txtheight1"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 150px;"></asp:TextBox>
                        <asp:Label ID="lbl_trerr" runat="server" Text="" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; color: Red;"></asp:Label>
                        <asp:Label ID="lbl_tralert" runat="server" Text="" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; color: Green;"></asp:Label>
                        <table style="margin-left: 30px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staffcode" runat="server" Text="Staff Code" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stfcodetrans" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 200px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stfnametrans" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 200px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fcollege" runat="server" Text="From College" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fcollege" runat="server" ReadOnly="true" CssClass="textbox txtheight5"
                                        Style="font-weight: bold; background-color: #99ccdd; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_tocollege" runat="server" Text="To College" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_tocollege" runat="server" CssClass="textbox txtheight3"
                                        Style="height: 30px; width: 250px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_tocollege_onchange" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Position="Bottom">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_from" runat="server" Text="From Department" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fdept" runat="server" ReadOnly="true" CssClass="textbox txtheight5"
                                        Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; background-color: #99ccdd;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_tdept" runat="server" Text="To Department" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_tdept" runat="server" CssClass="textbox txtheight3" Style="height: 30px; width: 250px;"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_tdept_change" Position="Bottom">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fdesig" runat="server" Text="From Designation" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fdesig" runat="server" ReadOnly="true" CssClass="textbox txtheight5"
                                        Style="font-weight: bold; background-color: #99ccdd; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_tdesig" runat="server" Text="To Designation" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_tdesig" runat="server" CssClass="textbox txtheight3" Style="height: 30px; width: 250px;"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        AutoPostBack="true" Position="Bottom">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Label ID="lbl_trserr" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_transfersave" runat="server" Text="Transfer" OnClick="btn_transfersave_OnClick"
                                Style="font-weight: bold; border-radius: 6px; font-family: book antiqua; font-size: medium; background-color: #6699ee;" />
                            <asp:Button ID="btn_transupdate" runat="server" Text="Update" OnClick="btn_transupdate_OnClick"
                                Style="font-weight: bold; border-radius: 6px; font-family: book antiqua; font-size: medium; background-color: #6699ee;" />
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="Relieve" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 395px;"
                    OnClick="ImageButton2_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 830px; height: 350px;"
                    align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 275px; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Relieve</span>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>Staff Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stfrelcode" runat="server" CssClass="textbox textbox1 txtheight3"
                                            Font-Bold="true" Font-Names="Book Antiqua" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>Staff Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stfrelname" runat="server" CssClass="textbox textbox1 txtheight3"
                                            Font-Bold="true" Font-Names="Book Antiqua" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <fieldset style="width: 680px; margin-left: 30px; border-color: Gray; border-radius: 2px;">
                            <asp:Label ID="lbl_rldate" runat="server" Text=" Date" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_rldate" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_rldate" runat="server"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <span style="color: Red;">*</span>
                            <asp:RadioButton ID="rdb_resig" runat="server" Visible="false" AutoPostBack="true"
                                Checked="true" OnCheckedChanged="rdb_resig_OnCheckedChanged" Text="Resignation"
                                GroupName="ra" />
                            <asp:RadioButton ID="rdb_relieved" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_relieved_change" Text="Relieved" Checked="true"
                                GroupName="ra" />
                            <asp:RadioButton ID="rdb_disc" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_disc_change" Text="Discontinued" GroupName="ra" />
                            <asp:RadioButton ID="rdb_cancelrel" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_cancelrel_change" Text="Cancel Relieve/Discontinue"
                                GroupName="ra" />
                        </fieldset>
                        <br />
                        <asp:Label ID="lbl_remark" runat="server" Text="Remarks" Style="font-weight: bold; margin-left: 30px; font-family: book antiqua; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_remarks" runat="server" MaxLength="30" TextMode="singleline"
                            CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; height: 20px; width: 324px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender114" runat="server" TargetControlID="txt_remarks"
                            FilterType="custom,uppercaseletters,lowercaseletters" ValidChars="letters " />


                        <asp:Label ID="lblresigdate" runat="server" Text="Resignation Date" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_regnation" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender26" TargetControlID="txt_regnation" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Button ID="btn_relieve_save" Text="Save" runat="server" Visible="false" OnClick="btn_relieve_save_OnClick"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />

                        <asp:Button ID="btn_relieveupdate" Text="Update" runat="server" Visible="false" OnClick="btn_relieveupdate_OnClick"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:LinkButton ID="lnkShwStudDet" runat="server" Text="Show Child Details" Visible="false" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnkShwStudDet_Click"></asp:LinkButton>
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="lblrelerr" runat="server" Text="" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                        </center>

                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbisnotisepaystaff" runat="server" Text="Is Notice Pay Staff" AutoPostBack="true" Checked="false" OnCheckedChanged="noticepay_checkedchange" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_noticepay" Visible="false" runat="server" CssClass="textbox textbox1 txtheight3"
                                            Font-Bold="true" Font-Names="Book Antiqua" Width="200px"></asp:TextBox>
                                    </td>

                                </tr>
                            </table>
                        </center>


                    </div>
                </div>
            </div>
        </center>

        <center>
            <div id="Promotion" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 395px;"
                    OnClick="ImageButton3_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 760px; height: 440px;"
                    align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 370px; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Promotion</span>
                        </center>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Staff Code" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_promoscode" runat="server" Enabled="false" ReadOnly="true" OnTextChanged="txt_promoscode_OnTextChanged"
                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 135px;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_promoscode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_promodept" runat="server" Text="Department" Style="font-weight: bold; margin-left: 68px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_promodept" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 265px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_promoname" runat="server" Text="Name" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_promrname" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 35px; width: 180px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_promodesig" runat="server" Text="Designation" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_promodesig" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 265px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_promostype" runat="server" Text="Staff Type" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_promostype" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 175px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_experience" Visible="false" runat="server" Text="Experience" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_experience" runat="server" Visible="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_joindate" runat="server" Text="Joining Date" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium; margin-left: 16px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_joindate" runat="server" Enabled="false" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender0012" TargetControlID="txt_joindate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <fieldset style="width: 700px; margin-left: 20px; border-color: Gray; border-radius: 2px;">
                            <asp:Label ID="lbl_desigto" runat="server" Text="Designated To" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:DropDownList ID="ddl_desigto" runat="server" CssClass="textbox txtheight3" Style="height: 30px;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                Width="265" Position="Bottom">
                            </asp:DropDownList>
                            <asp:Label ID="lbl_promodate" runat="server" Text="Date Of Promotion" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_promodate" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender0013" TargetControlID="txt_promodate" runat="server"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <span style="color: Red;">*</span>
                        </fieldset>
                        <br />
                        <center>
                            <asp:Label ID="lblpromerr" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                ForeColor="Red" Font-Size="Medium"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_promosave" Text="Save" runat="server" OnClick="btn_promosave_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_promoupdate" Text="Update" runat="server" OnClick="btn_promoupdate_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="Increment" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 425px;"
                    OnClick="ImageButton4_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 890px; height: 480px;"
                    align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 840px; height: 400px; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Increment</span>
                        </center>
                        <br />
                        <br />
                        <asp:Label ID="lbl_incrdate" runat="server" Text="Date Of Increment" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_incrdate" runat="server" CssClass="textbox txtheight1" Width="74px" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender014" TargetControlID="txt_incrdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <span style="color: Red;">*</span>
                        <asp:RadioButton ID="cb_basic" runat="server" AutoPostBack="true" OnCheckedChanged="cb_basic_OncheckedChanged"
                            Text="Basic" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"
                            GroupName="b" />
                        <asp:RadioButton ID="cbBasicPlusgrosspay" runat="server" AutoPostBack="true" OnCheckedChanged="cb_basic_OncheckedChanged"
                            Text="Basic+Grade" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"
                            GroupName="b" />
                        <fieldset id="basic" runat="server" style="width: 475px; margin-left: 438px; margin-top: -38px; border-color: Gray; border-radius: 2px; height: 25px;">
                            <asp:RadioButton ID="rdb_amt" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="rdb_amt_OnCheckedChanged"
                                Text="Amount" GroupName="ab" Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                            <asp:TextBox ID="txt_amt" runat="server" MaxLength="10" placeholder="0.00" CssClass="textbox txtheight1"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txt_amt"
                                FilterType="custom,Numbers" ValidChars="." />
                            <asp:RadioButton ID="rdb_percent" runat="server" AutoPostBack="true" Text="Percent"
                                OnCheckedChanged="rdb_percent_OnCheckedChanged" GroupName="ab" Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                            <asp:TextBox ID="txt_percent" runat="server" MaxLength="4" placeholder="0.00" CssClass="textbox txtheight1"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" Width="94px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" TargetControlID="txt_percent"
                                FilterType="custom,Numbers" ValidChars="." />
                        </fieldset>
                        <br />
                        <center>
                            <asp:Label ID="incr_alert" runat="server" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <farpoint:fpspread id="Fpspread2" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="820px" height="175px" class="spreadborder"
                                visible="false" showheaderselection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_incrsave" Text="Save" runat="server" OnClick="btn_incrsave_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_incrupdate" Text="Update" runat="server" OnClick="btn_incrupdate_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="Appraisal" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%; margin-top: -20px;">
                <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 35px; margin-left: 395px;"
                    OnClick="ImageButton5_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 840px; height: 580px;"
                    align="center">
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 530px; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Appraisal</span>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_apprscode" runat="server" Text="Staff Code" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprscode" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 11px;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_apprscode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_apprsname" runat="server" Text="Name" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprsname" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 175px; margin-left: 45px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_apprstype" runat="server" Text="Staff Type" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprstype" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 16px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_apprdesig" runat="server" Text="Designation" Style="font-weight: bold; margin-left: 20px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprdesig" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_apprdept" runat="server" Text="Department" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprdept" runat="server" Enabled="false" ReadOnly="true" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 175px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_apprjdate" runat="server" Text="Appraisal Date" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_apprjdate" runat="server" Enabled="false" CssClass="textbox txtheight1"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender05" TargetControlID="txt_apprjdate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:LinkButton ID="link_question" runat="server" Visible="false" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="medium" CssClass="lnkstyle" CausesValidation="False"
                            OnClick="link_question_OnClick" Style="margin-left: 27px;">Question</asp:LinkButton>
                        <asp:Button ID="btn_qplus" Text=" + " runat="server" Visible="false" OnClick="btn_qplus_OnClick"
                            Style="font-weight: bold; margin-left: 30px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:DropDownList ID="ddl_question" runat="server" Visible="false" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" Width="150">
                        </asp:DropDownList>
                        <asp:Button ID="btn_qminus" Text=" - " runat="server" OnClick="btn_qminus_OnClick"
                            Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <br />
                        <asp:LinkButton ID="link_answer" runat="server" Visible="false" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="medium" CssClass="lnkstyle" CausesValidation="False"
                            OnClick="link_answer_OnClick" Style="margin-left: 27px;">Answer</asp:LinkButton>
                        <asp:Button ID="btn_ansplus" Text=" + " runat="server" Visible="false" OnClick="btn_ansplus_OnClick"
                            Style="font-weight: bold; margin-left: 39px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:DropDownList ID="ddl_ans" runat="server" Visible="false" Font-Size="Medium"
                            AutoPostBack="true" Width="150">
                        </asp:DropDownList>
                        <asp:Button ID="btn_ansminus" Text=" - " runat="server" Visible="false" OnClick="btn_ansminus_OnClick"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <br />
                        <asp:Button ID="btnaddnewrow" runat="server" Text="Add Row" CssClass="textbox textbox1 btn2"
                            OnClick="btnaddnewrow_click" Style="margin-left: 660px; font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <br />
                        <center>
                            <farpoint:fpspread id="Fpspread3" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="724px" height="200px" class="spreadborder"
                                onbuttoncommand="Fpspread3_OnButtonCommand" visible="false" showheaderselection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                        </center>
                        <br />
                        <asp:Label ID="lbl_stat" runat="server" Text="Status" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:Label>
                        <asp:DropDownList ID="ddl_stat" runat="server" Style="font-weight: bold; font-family: book antiqua; font-size: medium;">
                            <asp:ListItem>Select</asp:ListItem>
                            <asp:ListItem Value="0">Suggested</asp:ListItem>
                            <asp:ListItem Value="1">Not Suggested</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btn_appr_save" Text="Save" runat="server" Visible="false" OnClick="btn_appr_save_OnClick"
                            Style="font-weight: bold; margin-left: 70px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:Button ID="btn_apprupdate" Text="Update" runat="server" Visible="false" OnClick="btn_apprupdate_OnClick"
                            Style="font-weight: bold; margin-left: 70px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:Button ID="btn_addq" Text="Add Questions" runat="server" OnClick="btn_addq_save_OnClick"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        <asp:Button ID="btn_hide" Text="Hide" runat="server" Visible="false" OnClick="btn_hide_save_OnClick"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <fieldset id="App_div" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%; margin-top: -20px;">
                <asp:ImageButton ID="ImageButton12" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 453px;"
                    OnClick="ImageButton12_Click" />
                <div class="subdivstyle" style="background-color: White; margin-left: -26px; overflow: auto; margin-top: 13px; width: 980px; height: 585px;"
                    align="center">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: indigo;">Staff Application</span>
                        </div>
                    </center>
                    <br />
                    <div>
                        <center>
                            <asp:Label ID="lblsemerror" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                Font-Bold="true" Font-Size="Medium" Style="position: absolute; margin-left: 678px;"></asp:Label>
                            <table style="top: 130px; margin-left: -490px; width: 450px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcollege1" runat="server" Text="College Name" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollege1" runat="server" Width="320px" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlcollege1_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lbl_aplalert" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Style="color: Red;"></asp:Label>
                            <fieldset style="top: 16px; height: 270px; width: 1080px; border-color: Olive; background-color: #99ffcc; border-bottom-width: 2px;">
                                <legend style="font-size: medium; font-family: Book Antiqua; font-weight: bold; border-color: Black;">Personal Information</legend>
                                <table class="tabl" style="top: 176px; margin-left: -715px; width: 330px; border-color: Gray; border-width: thin; height: 200px;">
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblenqno" runat="server" Text="Appl No" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:Label ID="Label86" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                Font-Bold="false"></asp:Label>
                                            <asp:TextBox ID="tbappno" runat="server" OnTextChanged="tbappno_OnTextChanged" AutoPostBack="true"
                                                CssClass="textbox txtheight1 txtupper" MaxLength="10" Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 185px; margin-left: -2px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="tbappno"
                                                FilterType="Custom,lowercaseletters,uppercaseletters,Numbers" ValidChars=",/-()" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lbltitle" runat="server" Text="Title:" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style25"></td>
                                        <td>&nbsp;
                                            <asp:DropDownList ID="ddlstitle" runat="server" OnSelectedIndexChanged="ddlstitle_OnSelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="true"
                                                CssClass="textbox ddlstyle ddlheight3">
                                                <asp:ListItem>Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style27">
                                            <asp:Label ID="lbltitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txt_firstname" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <td class="style5">
                                                    <asp:Label ID="lbladdname" runat="server" Text="First Name" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style55"></td>
                                                <td class="style25">
                                                    <asp:Label ID="lblstar" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                        Font-Bold="false"></asp:Label>
                                                    <asp:TextBox ID="txt_firstname" runat="server" CssClass="textbox txtheight1" MaxLength="100"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 185px; margin-left: -2px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender60" runat="server" TargetControlID="txt_firstname"
                                                        FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=". " />
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_firstname"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td></td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txt_middlename" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <td class="style5">
                                                    <asp:Label ID="lblmidlename" runat="server" Text="Middle Name" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style55"></td>
                                                <td class="style25">&nbsp;
                                                    <asp:TextBox ID="txt_middlename" runat="server" MaxLength="100" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 185px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender61" runat="server" TargetControlID="txt_middlename"
                                                        FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=". " />
                                                </td>
                                                <td></td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txt_surname1" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <td class="style5">
                                                    <asp:Label ID="lblsurname" runat="server" Text="SurName" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style55"></td>
                                                <td class="style25">&nbsp;
                                                    <asp:TextBox ID="txt_surname1" runat="server" MaxLength="100" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 185px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender62" runat="server" TargetControlID="txt_surname1"
                                                        FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=". " />
                                                </td>
                                                <td></td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblfathername" runat="server" Text="Father Name" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:Label ID="lblfatherstar" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                Font-Bold="false"></asp:Label>
                                            <asp:TextBox ID="txt_fathername" runat="server" MaxLength="100" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 185px; margin-left: -2px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender63" runat="server" TargetControlID="txt_fathername"
                                                FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=". " />
                                            <asp:Label ID="lblfathervalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblgender" runat="server" Text="Gender" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:RadioButton ID="rbmale" Text="Male" runat="server" GroupName="sex" Font-Names="Book Antiqua"
                                                Checked="true" Font-Size="Medium" />
                                            <asp:RadioButton ID="rbfemale" Text="Female" runat="server" GroupName="sex" Font-Names="Book Antiqua"
                                                Font-Size="Medium" />
                                        </td>
                                        <td class="style55"></td>
                                    </tr>
                                </table>
                                <table class="tabl" style="margin-top: -273px; margin-left: -31px; width: 330px; border-color: Gray; border-width: thin; height: 200px;">
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lbldob" runat="server" Text="Date of Birth" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txt_dop" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label84" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                        Font-Bold="false"></asp:Label>
                                                    <asp:TextBox ID="txt_dop" runat="server" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -2px;"></asp:TextBox>
                                                    <asp:Label ID="lblvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                        Font-Size="5pt" Visible="false"></asp:Label>
                                                    <asp:CalendarExtender ID="CalendarExtender17" TargetControlID="txt_dop" Format="dd/MM/yyyy"
                                                        runat="server" Enabled="True">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="style55"></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblbgroup" runat="server" Text="Blood Group" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style5">
                                            <asp:DropDownList ID="ddlbgroup" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" Style="margin-left: 9px;" OnSelectedIndexChanged="ddlbgroup_OnSelectedIndexChanged"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbgroupvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblmstatus" runat="server" Text="Marital Status" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlmstatus" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Style="margin-left: 0px;" CssClass="textbox ddlstyle ddlheight3">
                                                <asp:ListItem Text="Single" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Married" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Widowed" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Divorced" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Separated" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblmstatusvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblappdate" runat="server" Text="Applied Date" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:Label ID="lblastar" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                Font-Bold="false"></asp:Label>
                                            <asp:TextBox ID="txt_appdate" runat="server" AutoPostBack="true" OnTextChanged="txt_appdate_Change"
                                                CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -2px;"></asp:TextBox>
                                            <asp:Label ID="lblappdatevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                            <asp:CalendarExtender ID="CalendarExtender18" TargetControlID="txt_appdate" Format="dd/MM/yyyy"
                                                runat="server" Enabled="True">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="style55"></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblreligion" runat="server" Text="Religion" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlreligion" runat="server" OnSelectedIndexChanged="ddlreligion_OnSelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblreligionvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblcommunity" runat="server" Text="Community" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlcommunity" runat="server" OnSelectedIndexChanged="ddlcommunity_OnSelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblcommvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblcaste" runat="server" Text="Caste" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlcaste" runat="server" OnSelectedIndexChanged="ddlcaste_OnSelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblcastevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <br />
                                    <br />
                                </table>
                                <table class="tabl" style="margin-top: -242px; margin-left: 700px; width: 380px; border-color: Gray; border-width: thin; height: 200px;">
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblnationality" runat="server" Text="Nationality" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlnationality" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3" OnSelectedIndexChanged="ddlnationality_OnSelectedIndexChanged"
                                                Style="margin-left: -15px; width: 175px;">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblnationalityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblexp" runat="server" Text="Experience" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddl_experience" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: -2px; width: 175px;">
                                                <asp:ListItem Value="0">Fresher</asp:ListItem>
                                                <asp:ListItem Value="1">Experienced</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblpapplide" runat="server" Text="Post Applied for" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:Label ID="lblpoststar" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                Style="margin-left: -15px;" Font-Bold="false"> </asp:Label>
                                            <asp:DropDownList ID="ddlpapplide" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" CssClass="textbox ddlstyle ddlheight3" Style="width: 230px;">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblpapplidevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lbldeptapplied" runat="server" Text="Department Applied for" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:Label ID="lbldeptstar" runat="server" ForeColor="Red" Text="*" Font-Size="Medium"
                                                Style="margin-left: -15px;" Font-Bold="false"></asp:Label>
                                            <asp:DropDownList ID="ddldeptapplied" runat="server" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldeptapplied_OnSelectedIndexChanged"
                                                AutoPostBack="true" Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3"
                                                Style="width: 230px;">
                                            </asp:DropDownList>
                                            <asp:Label ID="lbldappliedvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblfsubject" runat="server" Text="Familiar Subjects" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55">
                                            <asp:Button ID="btnfstubjectadd" runat="server" Visible="false" Text="+" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                OnClick="btnfstubjectadd_Click" />
                                        </td>
                                        <td class="style25">&nbsp;
                                            <asp:DropDownList ID="ddlfsubjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlfsubjects_OnSelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="margin-left: -12px; width: 150px;"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblfsubjectsvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                Font-Size="5pt" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnfsubjectremove" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -25px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                OnClick="btnfsubjectremove_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblhandicap" runat="server" Text="Physically Handicapped" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:RadioButton ID="rbyes" Text="Yes" runat="server" GroupName="phandicapped" Font-Names="Book Antiqua"
                                                Font-Size="Medium" />
                                            <asp:RadioButton ID="rbno" Text="No" runat="server" GroupName="phandicapped" Font-Names="Book Antiqua"
                                                Font-Size="Medium" />
                                        </td>
                                        <td class="style55"></td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lbllang" runat="server" Text="Language Known" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style55"></td>
                                        <td class="style25">
                                            <asp:DropDownList ID="ddllang" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddllang_Change"
                                                CssClass="textbox ddlstyle ddlheight3" Style="margin-left: -2px; width: 175px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style55"></td>
                                    </tr>
                                </table>
                            </fieldset>
                            <asp:Panel ID="paddqualification" runat="server" Visible="False" Style="width: 200px; height: 100px; top: 322px; left: 200px; position: absolute;"
                                BorderStyle="Solid"
                                BorderWidth="1px" BackColor="#CCCCCC" CssClass="textbox txtheight1 " Font-Size="Medium">
                                <center>
                                    <caption runat="server" id="newcaption" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                    </caption>
                                    <br />
                                    <asp:TextBox ID="txt_addrelation" Width="200px" runat="server" CssClass="textbox txtheight1 txtcapitalize"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txt_addrelation"
                                        FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                    <br />
                                    <asp:Button ID="addnew" Width="50px" runat="server" Text="Add" OnClick="addnew_Click"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    &nbsp;
                                    <asp:Button ID="exitnew" Width="50px" runat="server" Text="Exit" OnClick="exitnew_Click"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                </center>
                            </asp:Panel>
                        </center>
                    </div>
                    <br />
                    <div align="left" style="overflow: auto; width: 1070px; height: auto; border-radius: 0px; border: 1px solid Gray; margin-left: 5px;">
                        <asp:Panel runat="server" ID="paneladd">
                            <asp:UpdatePanel ID="personalupdate" runat="server" style="height: 0px; margin-top: 240px;">
                                <ContentTemplate>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <br />
                        <div>
                            <asp:Panel ID="panelcontactcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Style="margin-top: -250px;" Width="1034px">
                                <asp:Label ID="lblcontactcollaps" Text="Contact" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagecontactcollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="Panel5" runat="server" Height="400px">
                            <asp:Label ID="lblerrorcontactpage" runat="server" Font-Names="Book Antiqua" Font-Size="5pt"
                                Font-Bold="true" ForeColor="Red" Text="Errorlabel" Visible="false" Style="position: absolute; left: -22px;">
                            </asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="permantpanel" runat="server" Style="border-color: Gray; border-width: thin;">
                                            <table class="tabl" style="width: 500px; height: 300px; background-color: #ffccff; margin-left: 30px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" runat="server" Style="color: #191970; font-weight: bold;" Text="Permanent address"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%-- <caption style="color: #191970; font-weight: bold;">
                                                    Permanent Address
                                                </caption>--%>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:Button ID="btnmove" runat="server" Text=">>" Font-Bold="True" Height="25px"
                                                            Width="30px" Font-Names="Book Antiqua" Font-Size="small" OnClick="btnmove_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="lblpaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                            Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td class="style25">
                                                        <asp:Label ID="lbladdressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                        <asp:TextBox ID="txt_paddress" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                            CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fte1" runat="server" TargetControlID="txt_paddress"
                                                            FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpstreet" runat="server" Text="Street" Font-Names="Book Antiqua"
                                                                    Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblstreetstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pstreet" runat="server" MaxLength="25" Width="250px" CssClass="textbox txtheight1"
                                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExender20" runat="server" TargetControlID="txt_pstreet"
                                                                    FilterType="UppercaseLetters,Lowercaseletters,Custom" ValidChars="/,.() ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblpcity" runat="server" Text="City" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnpcityadd" runat="server" Text="+" Style="display: none; top: 722px; left: 47px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btnpcityadd_Click" />
                                                        <asp:DropDownList ID="ddlpcity" runat="server" OnSelectedIndexChanged="ddlpcity_OnSelectedIndexChanged"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="true"
                                                            Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnpcityremove" runat="server" Text="-" Style="display: none; top: 722px; left: 207px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btnpcityremove_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblpdistirct" runat="server" Text="District" Font-Names="Book Antiqua"
                                                            Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnpdistirctadd" runat="server" Text="+" Style="display: none; top: 748px; left: 47px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btnpdistirctadd_Click" />
                                                        <asp:DropDownList ID="ddlpdistirct" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            Font-Bold="true" Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btndistirctremove" runat="server" Text="-" Style="display: none; top: 748px; left: 207px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btndistirctremove_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="lblpstate" runat="server" Text="State" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td class="style25">
                                                        <asp:Button ID="btnpstateadd" runat="server" Text="+" Style="display: none; top: 775px; left: 47px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btnpstateadd_Click" />
                                                        <asp:DropDownList ID="ddlpstate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            Font-Bold="true" Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnpstateremove" runat="server" Text="-" Style="display: none; top: 775px; left: 207px; position: absolute;"
                                                            Font-Names="Book Antiqua" Font-Size="Small"
                                                            Height="21px" OnClick="btnpstateremove_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lblppincode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                                                    Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblppincodestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_ppincode" runat="server" Font-Names="Book Antiqua" MaxLength="6"
                                                                    Font-Bold="true" Width="250px" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_ppincode"
                                                                    FilterType="Numbers,custom" ValidChars=" -">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lbllandline" runat="server" Text="Res Phone" Font-Names="Book Antiqua"
                                                                    Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55">
                                                                <asp:TextBox ID="txt_presphone" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                    MaxLength="12" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_presphone"
                                                                    FilterType="Custom,Numbers" ValidChars="-">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel32" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpmobile" runat="server" Text="Mobile No" Font-Names="Book Antiqua"
                                                                    Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpmobilestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pmobile" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                    MaxLength="11" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_pmobile"
                                                                    FilterType="Numbers">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel33" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpfax" runat="server" Text="Fax:" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpfaxstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pfax" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                    MaxLength="16" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txt_pfax"
                                                                    FilterType="Numbers,Custom" ValidChars="-+()">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="UpdatePanel34" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpemail" runat="server" Text="Email" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55">
                                                                <asp:TextBox ID="txt_pemail" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                    CssClass="textbox txtheight1 txtnone" onblur="return checkEmail(this)" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblvalidationpaddress" runat="server" Text="" Visible="false" ForeColor="Red"
                                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <table class="tabl" style="width: 450px; border-color: Gray; margin-top: 3px; margin-left: 20px; border-width: thin; height: 300px; background-color: #ffccff;">
                                            <caption style="color: #191970; font-weight: bold;">
                                                Communication Address
                                            </caption>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:Button ID="btncommmove" runat="server" Text=">>" Font-Bold="True" Height="25px"
                                                        Width="30px" Font-Names="Book Antiqua" Font-Size="Small" OnClick="btncommmove_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel35" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_caddress" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                                TextMode="MultiLine" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txt_caddress"
                                                                FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel36" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcstreet" runat="server" Text="Street" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcstreetstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cstreet" runat="server" MaxLength="25" Width="250px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_cstreet"
                                                                FilterType="lowercaseletters,uppercaseletters,Custom" ValidChars="/.,() ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblccity" runat="server" Text="City" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:Button ID="btnccityadd" runat="server" Text="+" Style="display: none; top: 124px; left: 43px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btnccityadd_Click" />
                                                    <asp:DropDownList ID="ddlccity" runat="server" OnSelectedIndexChanged="ddlccity_OnSelectedIndexChanged"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="true"
                                                        Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnccityremove" runat="server" Text="-" Style="display: none; top: 124px; left: 203px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btnccityremove_Click" />
                                                </td>
                                                <td class="style27">
                                                    <asp:Label ID="lblccityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                        Visible="false" Font-Size="5pt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcdistrict" runat="server" Text="District" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btncdistrictadd" runat="server" Text="+" Style="display: none; top: 150px; left: 43px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btncdistrictadd_Click" />
                                                    <asp:DropDownList ID="ddlcdistrict" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Font-Bold="true" Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btncdistrictremove" runat="server" Text="-" Style="display: none; top: 150px; left: 203px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btncdistrictremove_Click" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcdistrictvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                        Visible="false" Font-Size="5pt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblcstate" runat="server" Text="State" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:Button ID="btncstateadd" runat="server" Text="+" Style="display: none; top: 176px; left: 43px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btncstateadd_Click" />
                                                    <asp:DropDownList ID="ddlcstate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Font-Bold="true" Width="200px" CssClass="textbox ddlstyle ddlheight3" Style="margin-left: 20px;">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btncstateremove" runat="server" Text="-" Style="display: none; top: 176px; left: 203px; position: absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="21px" OnClick="btncstateremove_Click" />
                                                </td>
                                                <td class="style27">
                                                    <asp:Label ID="lblcstatevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                        Visible="false" Font-Size="5pt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel37" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcpincode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcpincodestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cpincode" runat="server" MaxLength="6" Width="250px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_cpincode"
                                                                FilterType="Numbers,Custom" ValidChars=" -">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel38" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcresphone" runat="server" Text="Res Phone" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcresphonestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cresphone" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                MaxLength="12" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txt_cresphone"
                                                                FilterType="Numbers" ValidChars="-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel39" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcmobileno" runat="server" Text="Mobile No" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcmobilenostar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cmobileno" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                MaxLength="11" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txt_cmobileno"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel40" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcfax" runat="server" Text="Fax:" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcfaxstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cfax" runat="server" Width="250px" Font-Names="Book Antiqua"
                                                                MaxLength="16" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender031" runat="server" TargetControlID="txt_cfax"
                                                                FilterType="Numbers,Custom" ValidChars="-+()">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <asp:UpdatePanel ID="UpdatePanel41" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td class="style5">
                                                            <asp:Label ID="lblcremarks" runat="server" Text="Remarks:" Font-Names="Book Antiqua"
                                                                Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblcremarksstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_cremarks" runat="server" Width="250px" TextMode="MultiLine"
                                                                CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender64" runat="server" TargetControlID="txt_cremarks"
                                                                FilterType="lowercaseletters,uppercaseletters,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblvalidationcaddress" runat="server" Visible="false" ForeColor="Red"
                                                        Text="" Font-Size="Medium" Font-Names="Book Antiqua" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnk_guard" runat="server" Visible="true" OnClick="lnk_guard_OnClick"
                                            Font-Bold="true" Font-Names="Book Antiqua" CausesValidation="False" Style="margin-left: 395px;">Guardian Address</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnl_referee" runat="server" Visible="true" OnClick="lnl_referee_OnClick"
                                            Font-Bold="true" Font-Names="Book Antiqua" CausesValidation="False" Style="margin-left: 350px;">Referee Address</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <div id="Popdiv" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <div id="Div11" runat="server" class="table" style="background-color: White; height: 495px; width: 600px; border: 5px solid #3399ff; border-top: 25px solid #3399ff; margin-top: 50px; border-radius: 10px;">
                                            <br />
                                            <center>
                                                <asp:Label ID="lbl_contactheader" runat="server" ForeColor="Indigo" Text="" Font-Size="Large"
                                                    Font-Bold="true"></asp:Label>
                                            </center>
                                            <br />
                                            <center>
                                                <table id="guard" runat="server" visible="false" class="tabl" style="width: 400px; border-color: Gray; top: 597px; left: 492px; border-width: thin; height: 320px; background-color: #ffccff; margin-top: 0px;">

                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label12" runat="server" Style="color: #191970; font-weight: bold;" Text="Guardian address"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <%-- <caption style="color: #191970; font-weight: bold;">
                                                        Guardian address</caption>--%>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgname" runat="server" Text="Guardian Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgnamestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gname" runat="server" Width="200px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="txt_gname"
                                                                FilterType="lowercaseletters,Uppercaseletters,Custom" ValidChars="., ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gaddress" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                                TextMode="MultiLine" CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" TargetControlID="txt_gaddress"
                                                                FilterType="Numbers,lowercaseletters,Uppercaseletters,Custom" ValidChars="/,.() ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgstreet" runat="server" Text="Street" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgstreetstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gstreet" runat="server" MaxLength="25" Width="200px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" TargetControlID="txt_gstreet"
                                                                FilterType="lowercaseletters,uppercaseletters,Custom" ValidChars="- ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgcity" runat="server" Text="City" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Button ID="btngcityadd" runat="server" Text="+" Style="display: none; top: 141px; left: 44px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngcityadd_Click" />
                                                            <asp:DropDownList ID="ddlgcity" runat="server" OnSelectedIndexChanged="ddlgcity_OnSelectedIndexChanged"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="true"
                                                                Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btngcityremove" runat="server" Text="-" Style="display: none; top: 141px; left: 203px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngcityremove_Click" />
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Label ID="lblcityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblgdistrict" runat="server" Text="District" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btngdistrictadd" runat="server" Text="+" Style="display: none; top: 169px; left: 44px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngdistrictadd_Click" />
                                                            <asp:DropDownList ID="ddlgdistrict" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                                Font-Size="Medium" Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btngdistrictremove" runat="server" Text="-" Style="display: none; top: 169px; left: 203px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngdistrictremove_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblgdistrictvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgstate" runat="server" Text="State" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Button ID="btngstateadd" runat="server" Text="+" Style="display: none; top: 195px; left: 44px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngstateadd_Click" />
                                                            <asp:DropDownList ID="ddlgstate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Font-Bold="true" Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btngstateremove" runat="server" Text="-" Style="display: none; top: 195px; left: 203px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngstateremove_Click" />
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Label ID="lblgstatevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgpincode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgpincodestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gpincode" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                MaxLength="7" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" TargetControlID="txt_gpincode"
                                                                FilterType="Numbers" ValidChars=" -">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgresphone" runat="server" Text="Res Phone" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgresphonestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gresphone" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                MaxLength="10" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" TargetControlID="txt_gresphone"
                                                                FilterType="Numbers,Custom" ValidChars="-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgmobileno" runat="server" Text="Mobile No" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblgmobilenostar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_gmobileno" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                MaxLength="11" CssClass="textbox txtheight1 " Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txt_gmobileno"
                                                                FilterType="Numbers,Custom" ValidChars="-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblgrelation" runat="server" Text="Relation" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Button ID="btngrelationadd" runat="server" Text="+" Style="display: none; top: 341px; left: 44px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngrelationadd_Click" />
                                                            <asp:DropDownList ID="ddlgrelation" runat="server" OnSelectedIndexChanged="ddlgrelation_OnSelectedIndexChanged"
                                                                AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                                                                Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btngrelationremove" runat="server" Text="-" Style="display: none; top: 341px; left: 203px; position: absolute;"
                                                                Font-Names="Book Antiqua" Font-Size="Small"
                                                                Height="21px" OnClick="btngrelationremove_Click" />
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Label ID="lblgrelationvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="lblvalidataiongaddress" runat="server" Text="" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Visible="false" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="referee" runat="server" class="tabl" visible="false" style="width: 400px; border-color: Gray; border-width: 2px; top: 597px; left: 739px; border-width: thin; height: 300px; background-color: #ffccff; margin-top: 0px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label14" runat="server" Text="Referee Address" Style="color: #191970; font-weight: bold;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <%--<caption style="color: #191970; font-weight: bold;">
                                                        Referee Address
                                                    </caption>--%>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefname" runat="server" Text="Referee Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefnamestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refname" runat="server" Width="200px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" TargetControlID="txt_refname"
                                                                FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars="., ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefaddress" runat="server" Text="Referee Address" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefaddressstar" runat="server" Visible="false" ForeColor="Red"
                                                                Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refaddress" runat="server" MaxLength="100" Width="250px" TextMode="MultiLine"
                                                                CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" TargetControlID="txt_refaddress"
                                                                FilterType="Custom,Numbers,lowercaseLetters,Uppercaseletters" ValidChars="/-,.() ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefstreet" runat="server" Text="Street" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefstreetstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refstreet" runat="server" MaxLength="25" Width="200px" CssClass="textbox txtheight1"
                                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" TargetControlID="txt_refstreet"
                                                                FilterType="lowercaseLetters,Uppercaseletters,Custom" ValidChars="- ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefcity" runat="server" Text="City" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:DropDownList ID="ddlrefcity" runat="server" OnSelectedIndexChanged="ddlrefcity_OnSelectedIndexChanged"
                                                                AutoPostBack="true" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                                                Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Label ID="lblrefcityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblrdistrict" runat="server" Text="District" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlrdistrict" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrdistrict_Change"
                                                                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblrdistrictvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefstate" runat="server" Text="State" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:DropDownList ID="ddlrefstate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrefstate_Change"
                                                                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Width="175px" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Label ID="lblrefstatevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefpincode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefpincodstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refpincode" runat="server" Width="200px" CssClass="textbox txtheight1 "
                                                                MaxLength="7" Font-Size="Medium" Height="16px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" TargetControlID="txt_refpincode"
                                                                FilterType="Numbers" ValidChars=" -">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefresphone" runat="server" Text="Res Phone" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefreshpone1" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refresphoneno" runat="server" CssClass="textbox txtheight1 "
                                                                MaxLength="10" Font-Size="Medium" Height="16px" Width="200px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender042" runat="server" TargetControlID="txt_refresphoneno"
                                                                FilterType="Numbers,Custom" ValidChars="-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefresmobileno" runat="server" Text="Mobile No" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefresmobilenostar" runat="server" Visible="false" ForeColor="Red"
                                                                Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refresmobileno" runat="server" Width="200px" CssClass="textbox txtheight1 "
                                                                MaxLength="11" Font-Size="Medium" Height="16px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender43" runat="server" TargetControlID="txt_refresmobileno"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefcompany" runat="server" Text="Company Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefcompanystar" runat="server" Visible="false" ForeColor="Red"
                                                                Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_refcompany" runat="server" MaxLength="25" CssClass="textbox txtheight1"
                                                                Font-Size="Medium" Height="16px" Width="200px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_refcompany"
                                                                FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars="-,.">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblrefdesignation" runat="server" Text="Designation" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblrefdesignationstar" runat="server" Font-Bold="true" Visible="false"
                                                                ForeColor="Red" Text="*" Font-Size="Medium">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txt_refdesignation" runat="server" CssClass="textbox txtheight1"
                                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" Width="200px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_refdesignation"
                                                                FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars="-,.">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="lblvalidationrrraddress" runat="server" Text="" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Visible="false" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Button ID="btn_close" Text="Close" runat="server" OnClick="btn_close_OnClick"
                                                    Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                        </asp:Panel>
                        <asp:Panel ID="panelcontact" runat="server" Visible="False" Style="width: 200px; height: 100px; top: 130px; left: 395px; position: absolute;"
                            BorderStyle="Solid"
                            Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                            <center>
                                <caption runat="server" id="capcont" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txt_cont" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnaddcont" Width="50px" runat="server" Text="Add" OnClick="btnaddcont_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                &nbsp;
                                <asp:Button ID="btnexitcont" Width="50px" runat="server" Text="Exit" OnClick="btnexitcont_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                            </center>
                        </asp:Panel>
                        <asp:Panel ID="panelcom" runat="server" Visible="False" Style="width: 200px; height: 100px; top: 130px; left: 395px; position: absolute;"
                            BorderStyle="Solid" Font-Bold="true"
                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium">
                            <center>
                                <caption runat="server" id="capcom" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txt_com" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnaddcom" Width="50px" runat="server" Text="Add" OnClick="btnaddcom_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                &nbsp;
                                <asp:Button ID="btnexitcom" Width="50px" runat="server" Text="Exit" OnClick="btnexitcom_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                            </center>
                        </asp:Panel>
                        <asp:Panel ID="panelguard" runat="server" Visible="False" Style="width: 200px; height: 100px; top: 130px; left: 395px; position: absolute;"
                            BorderStyle="Solid" Font-Bold="true"
                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium">
                            <center>
                                <caption runat="server" id="capguard" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txt_guard" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnaddguard" Width="50px" runat="server" Text="Add" OnClick="btnaddguard_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                &nbsp;
                                <asp:Button ID="btnexitguard" Width="50px" runat="server" Text="Exit" OnClick="btnexitguard_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                            </center>
                        </asp:Panel>
                        <asp:Panel ID="panelref" runat="server" Visible="False" Style="width: 200px; height: 100px; top: 130px; left: 395px; position: absolute;"
                            BorderStyle="Solid" Font-Bold="true"
                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium">
                            <center>
                                <caption runat="server" id="capref" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txt_ref" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnaddref" Width="50px" runat="server" Text="Add" OnClick="btnaddref_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                &nbsp;
                                <asp:Button ID="btnexitref" Width="50px" runat="server" Text="Exit" OnClick="btnexitref_Click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender14" runat="server" TargetControlID="Panel5"
                            CollapseControlID="panelcontactcollaps" ExpandControlID="panelcontactcollaps"
                            Collapsed="true" TextLabelID="lblcontactcollaps" CollapsedSize="0" ImageControlID="imagecontactcollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelfamilycollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfamilycollaps" Text="Family Information" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefamilycollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="pa" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnaddfamily" runat="server" Text="Add" visble="false" Font-Names="Book Antiqua"
                                                Font-Bold="true" Style="margin-left: 800px;" Font-Size="Medium" OnClick="btnaddfamily_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Familyinfo_grid" OnRowDataBound="Familyinfo_grid_OnRowDataCommand"
                                                OnRowCommand="Familyinfo_grid_OnRowCommand" AutoGenerateColumns="false" GridLines="Both"
                                                runat="server" Style="border-radius: 10px;" HeaderStyle-BackColor="#3399ff">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_S_No" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlyname" runat="server" Text='<%#Eval("Familyname") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Age" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlyage" runat="server" Text='<%#Eval("Age") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gender" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlygender" runat="server" Text='<%#Eval("Gender") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qualification" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlyqulification" runat="server" Text='<%#Eval("Qualification") %>'></asp:Label>
                                                            <asp:Label ID="lbl_fmlyqulificationcode" Visible="false" runat="server" Text='<%#Eval("Qualification_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Relationship" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlyrelationship" runat="server" Text='<%#Eval("Relationship") %>'></asp:Label>
                                                            <asp:Label ID="lbl_fmlyrelationshipcode" runat="server" Visible="false" Text='<%#Eval("Relationship_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlystatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                            <asp:Label ID="lbl_statuscode" runat="server" Visible="false" Text='<%#Eval("Statuscode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Income" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fmlyincome" runat="server" Text='<%#Eval("Income") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <div id="FamilyInfo" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelfamily" runat="server" BorderColor="Black" BorderWidth="2px"
                                            BackColor="White" Font-Bold="true" Height="280px" Width="500px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Label ID="Label3" runat="server" Style="color: #191970;" Text="Family Information"
                                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrelation" runat="server" Text="Relationship" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnadd" runat="server" Text="+" Font-Names="Book Antiqua" Font-Size="Small"
                                                            Style="display: none;" Height="21px" OnClick="btnadd_Click" />
                                                        <asp:DropDownList ID="ddlrelationship" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrelationship_OnSelectedIndexChanged"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox ddlstyle ddlheight3">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnminus" runat="server" Text="-" Font-Names="Book Antiqua" Font-Size="Small"
                                                            Style="display: none;" Height="21px" OnClick="btnminus_Click" />
                                                        <asp:Label ID="lblrelationvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                            Font-Size="5pt" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrname" runat="server" Text="Name" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblnamestar" runat="server" ForeColor="Red" Text="*" Visible="false"
                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                        <asp:TextBox ID="txt_rname" runat="server" CssClass="textbox txtheight1 " MaxLength="20"
                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="200px"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender72" runat="server" TargetControlID="txt_rname"
                                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars="letters  ." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrage" runat="server" Text="Age" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblagestar" runat="server" ForeColor="Red" Text="*" Visible="false"
                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                        <asp:TextBox ID="txt_rage" runat="server" CssClass="textbox txtheight1 " MaxLength="2"
                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="200px"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender07" runat="server" TargetControlID="txt_rage"
                                                            FilterType="Custom,Numbers" ValidChars="0,1,2,3,4,5,6,7,8,9 " />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrqualification" runat="server" Text="Qualification" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnrqualificationadd" runat="server" Visible="false" Text="+" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                            OnClick="btnrqualificatinadd_Click" />
                                                        <asp:DropDownList ID="ddlrqualification" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                            Font-Bold="true" Font-Size="Medium" OnSelectedIndexChanged="ddlrqualification_SelectedIndexChanged"
                                                            Style="margin-left: 2px;" CssClass="textbox ddlstyle ddlheight3">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnrqualificationminus" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -75px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                            OnClick="btnrqualificationminus_Click" />
                                                        <asp:Label ID="lblrqualificationvalidation" runat="server" ForeColor="Red" Text=""
                                                            Visible="false" Font-Names="Book Antiqua" Font-Size="5pt"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrgender" runat="server" Text="Gender" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbrmale" Text="Male" runat="server" GroupName="rGender" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Checked="true" />
                                                        <asp:RadioButton ID="rbrfemale" Text="Female" runat="server" GroupName="rGender"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblstatus" runat="server" Text="Status" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnstatusadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                            OnClick="btnstatusadd_Click" />
                                                        <asp:DropDownList ID="ddlrstatus" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Font-Bold="true" OnSelectedIndexChanged="ddlrstatus_OnSelectedIndexChanged"
                                                            Style="margin-left: 2px;" CssClass="textbox ddlstyle ddlheight3">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnstatusminus" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                            OnClick="btnstatusminus_Click" />
                                                        <asp:Label ID="lblstatusvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                            Visible="false" Font-Size="5pt"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblincome" runat="server" Text="Income (per Month)" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblincomestar" runat="server" ForeColor="Red" Text="*" Visible="false"
                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                        <asp:TextBox ID="txt_rincome" runat="server" Font-Names="Book Antiqua" MaxLength="10"
                                                            CssClass="textbox txtheight1 " Font-Bold="true" Font-Size="Medium" Height="16px"
                                                            Width="200px"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender073" runat="server" TargetControlID="txt_rincome"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnfamilyadd" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                            Font-Bold="true" Font-Size="Medium" OnClick="btnfamilyadd_Click" />
                                                        <asp:Button ID="btnfailyupdate" runat="server" Text="Update" Font-Names="Book Antiqua"
                                                            Visible="false" Font-Bold="true" Font-Size="Medium" OnClick="btnfailyupdate_Click" />
                                                        <asp:Button ID="btn_fdelete" runat="server" Text="Delete" Font-Names="Book Antiqua"
                                                            Visible="false" Font-Bold="true" Font-Size="Medium" OnClick="btn_fdelete_Click" />
                                                        <asp:Button ID="btnfamilyexit" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                            Font-Bold="true" Font-Size="Medium" OnClick="btnfamilyexit_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblfamilyvalidation" runat="server" ForeColor="Red" Text="" Visible="false"
                                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelfamilyinformation" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capfamily" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_family" Width="154px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender043" runat="server" TargetControlID="txt_family"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnfamilyinfoadd" Width="50px" runat="server" Text="Add" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnfamilyinfoexit" Width="50px" runat="server" Text="Exit" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="collapsfamilypanel1" runat="server" TargetControlID="pa"
                            CollapseControlID="panelfamilycollaps" ExpandControlID="panelfamilycollaps" Collapsed="true"
                            TextLabelID="lblfamilycollaps" CollapsedSize="0" ImageControlID="imagefamilycollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelqulificationcolpas" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfilter1" Text="Qualification" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefilter1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderquli" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnquliadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnquliadd_Click"
                                                Style="margin-left: 800px;" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="qualification_grid" runat="server" OnRowDataBound="qualification_grid_OnRowDataCommand"
                                                OnRowCommand="qualification_grid_OnRowCommand" AutoGenerateColumns="false" GridLines="Both"
                                                Visible="false" HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_S_No" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Graduation" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_grad" runat="server" Text='<%#Eval("Graduation") %>'></asp:Label>
                                                            <asp:Label ID="lbl_gradcode" runat="server" Visible="false" Text='<%#Eval("Graduation_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_deg" runat="server" Text='<%#Eval("Degree") %>'></asp:Label>
                                                            <asp:Label ID="lbl_degcode" runat="server" Visible="false" Text='<%#Eval("Degree_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Specialization" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_special" runat="server" Text='<%#Eval("Specialization") %>'></asp:Label>
                                                            <asp:Label ID="lbl_specialcode" runat="server" Visible="false" Text='<%#Eval("Specialization_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Year Of Passing" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_yearofpass" runat="server" Text='<%#Eval("Year Of Passing") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="University" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_univerc" runat="server" Text='<%#Eval("University") %>'></asp:Label>
                                                            <asp:Label ID="lbl_univercity_code" runat="server" Visible="false" Text='<%#Eval("University_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Institution" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Institution" runat="server" Text='<%#Eval("Institution") %>'></asp:Label>
                                                            <asp:Label ID="lbl_Institutioncode" runat="server" Visible="false" Text='<%#Eval("Institution_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Percentage" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_percentage" runat="server" Text='<%#Eval("Percentage") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_grade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Class" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_class" runat="server" Text='<%#Eval("Class") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Qualification" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelqulificaiton" runat="server" BorderColor="Black" BackColor="White"
                                            Font-Bold="true" BorderWidth="2px" Height="390px" Width="470px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="Div4" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 30px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Label ID="lblheaderquli" runat="server" Style="color: #191970;" Text="Qualification"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblgraduation" runat="server" Text="Graduation" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btngraduationadd" runat="server" Visible="false" Text="+" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngraduationadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlgraduation" AutoPostBack="true" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" OnSelectedIndexChanged="ddlgraduation_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btngraduationminus" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngraduationminus_Click" />
                                                                <asp:Label ID="lblgraduationvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbldegreeinfromation" runat="server" Text="Degree" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btndegreeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btndegreeadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddldegreeinfromation" AutoPostBack="true" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" OnSelectedIndexChanged="ddldegreeinfromation_SelectedIndexChanged"
                                                                    Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btndegreeremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btndegreeremove_Click" />
                                                                <asp:Label ID="lbldegereevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblspecalization" runat="server" Text="Specialization" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnspecalizationadd" runat="server" Visible="false" Text="+" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnspecalizationadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlspecalization" AutoPostBack="true" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" OnSelectedIndexChanged="ddlspecalization_SelectedIndexChanged"
                                                                    Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnspecalizationremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnspecalizationremove_Click" />
                                                                <asp:Label ID="lblspecalizationvalidation" runat="server" ForeColor="Red" Text=""
                                                                    Font-Names="Book Antiqua" Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblyearofpassing" runat="server" Text="Year of Passing" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td class="style25">
                                                                <asp:TextBox ID="txt_yofp" runat="server" OnTextChanged="txt_yofp_Change" AutoPostBack="true"
                                                                    CssClass="textbox txtheight2" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                                <asp:CalendarExtender ID="caldatestart0" TargetControlID="txt_yofp" runat="server"
                                                                    Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Label ID="lblyearofvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbluniversity" runat="server" Text="University" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnunivesityadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnunivesityadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddluniversity" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" OnSelectedIndexChanged="ddluniversity_SelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnuniversityremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnuniversityremove_Click" />
                                                                <asp:Label ID="lbluniversityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblinstitution" runat="server" Text="Institution" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btninstitutionadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btninstitutionadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlinstitution" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" OnSelectedIndexChanged="ddlinstitution_SelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btninstitutionremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btninstitutionremove_Click" />
                                                                <asp:Label ID="lblinstitutionvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel039" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblpercentage" runat="server" Text="Percentage" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblpercentagestar" runat="server" Visible="false" ForeColor="Red"
                                                                            Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_percentage1" runat="server" CssClass="textbox txtheight1 " MaxLength="4"
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender71" runat="server" TargetControlID="txt_percentage1"
                                                                            FilterType="Custom,Numbers" ValidChars="%,/-() ." />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel040" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblgrade" runat="server" Text="Grade" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblgradestar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_grade" runat="server" CssClass="textbox txtheight1 " MaxLength="6"
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender70" runat="server" TargetControlID="txt_grade"
                                                                            FilterType="Custom,lowercaseletters,uppercaseletters,numbers" ValidChars=",/-() . " />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel041" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblclass" runat="server" Text="Class" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblcalssstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_class" runat="server" CssClass="textbox txtheight1 txtupper"
                                                                            MaxLength="15" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                                                            Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender69" runat="server" TargetControlID="txt_class"
                                                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>

                                                            <td colspan="4" align="center">
                                                                <asp:FileUpload ID="FileUpload2" runat="server" />

                                                                <asp:Button ID="btnquliUpload" runat="server" Text="Upload" Style="left: 740px; position: absolute; width: 68px;"
                                                                    OnClick="btnquliUpload_Click" />
                                                                <asp:LinkButton ID="lnkbtnquali" Text="DownloadAttachment" Font-Name="Book Antiqua" Font-Size="11pt"
                                                                    OnClick="lnkdownlaodattachement1_Click" runat="server" Style="left: 810px; position: absolute; Width: 2px;" />
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnaddquli" runat="server" Text="Add" Font-Size="Medium" OnClick="btnaddquli_Click" />
                                                                <asp:Button ID="btnupdatequli" runat="server" Text="Update" Visible="false" Font-Size="Medium"
                                                                    OnClick="btnupdatequli_Click" />
                                                                <asp:Button ID="btn_qdelete" runat="server" Text="Delete" Visible="false" Font-Size="Medium"
                                                                    OnClick="btn_qdelete_Click" />
                                                                <asp:Button ID="btnexitquli" runat="server" Text="Exit" Font-Size="Medium" OnClick="btnexitquli_Click" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblqulivalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelentry" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capquli" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_entryvalue" Width="150px" Height="16px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txt_entryvalue"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddentry" Width="50px" runat="server" Text="Add" OnClick="btnaddentry_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnremoveentry" Width="50px" runat="server" Text="Exit" OnClick="btnremoveentry_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="colpspanelex2" runat="server" TargetControlID="panelheaderquli"
                            CollapseControlID="panelqulificationcolpas" ExpandControlID="panelqulificationcolpas"
                            Collapsed="true" TextLabelID="lblfilter1" CollapsedSize="0" ImageControlID="imagefilter1"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelexperiencecollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfilter2" Text="Experience" runat="server" Font-Size="Large" Font-Bold="true"
                                    Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefilter2" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderexp" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnexpadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnexpadd_Click"
                                                Style="margin-left: 800px;" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Experience_grid" runat="server" Visible="false" OnRowDataBound="Experience_grid_OnRowDataCommand"
                                                OnRowCommand="Experience_grid_OnRowCommand" AutoGenerateColumns="false" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_S_No" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="College" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_College" runat="server" Text='<%#Eval("College") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_From" runat="server" Text='<%#Eval("From") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_To" runat="server" Text='<%#Eval("To") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Oraganization" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Oraganization" runat="server" Text='<%#Eval("Oraganization") %>'></asp:Label>
                                                            <asp:Label ID="lbl_Oraganization_code" runat="server" Visible="false" Text='<%#Eval("Oraganization_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Designation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                            <asp:Label ID="lbl_Designation_code" runat="server" Visible="false" Text='<%#Eval("Designation_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Experience in" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Experiencein" runat="server" Text='<%#Eval("Experience in") %>'></asp:Label>
                                                            <asp:Label ID="lbl_Experiencein_code" runat="server" Visible="false" Text='<%#Eval("Experiencein_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Year" runat="server" Text='<%#Eval("Year") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Month" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Salary" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Salary" runat="server" Text='<%#Eval("Salary") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reasons for relieving" HeaderStyle-BackColor="#3399ff"
                                                        HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Reasonsrelieving" runat="server" Text='<%#Eval("Reasons for relieving") %>'></asp:Label>
                                                            <asp:Label ID="lbl_Reasonsrelievingcode" runat="server" Visible="false" Text='<%#Eval("Reasonsrelieving_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duties" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Duties" runat="server" Text='<%#Eval("Duties") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Experience" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelexperience" runat="server" BorderColor="Black" BackColor="White"
                                            BorderWidth="2px" Style="background-color: #ffccff; border-radius: 10px; background-color: #ffccff; margin-top: 175px;"
                                            Height="400px" Width="600px">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_ex" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <caption style="color: #191970; font-weight: bold;">
                                                            Experience Details</caption>
                                                        <tr>
                                                            <td colspan="4" align="right">
                                                                <asp:RadioButton ID="rbowncollege" Text="Own College" runat="server" Checked="true"
                                                                    GroupName="college" AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    OnCheckedChanged="rbowncollege_CheckedChanged" />
                                                                <asp:RadioButton ID="rbothercollege" Text="Other College" runat="server" GroupName="college"
                                                                    AutoPostBack="true" Font-Names="Book Antiqua" OnCheckedChanged="rbothercollege_CheckedChanged"
                                                                    Font-Size="Medium" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblfrom" runat="server" Text="From" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fromdate" AutoPostBack="true" OnTextChanged="txt_fromdate_OnTextChanged"
                                                                    Height="16px" Width="85" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender023" runat="server" TargetControlID="txt_fromdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender08" Format="dd/MM/yyyy" TargetControlID="txt_fromdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblto" runat="server" Text="To" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                                <asp:TextBox ID="txt_todate" AutoPostBack="true" OnTextChanged="txt_todate_OnTextChanged"
                                                                    Height="16px" Width="85" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender024" runat="server" TargetControlID="txt_todate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender09" Format="dd/MM/yyyy" TargetControlID="txt_todate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_yearapp" runat="server" Text="Year" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_year" Height="16px" Width="80" runat="server" ReadOnly="true"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender65" runat="server" TargetControlID="txt_year"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" Text="Month" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                                <asp:TextBox ID="txt_month" Height="16px" Width="66px" runat="server" Font-Bold="True"
                                                                    ReadOnly="true" Font-Names="Book Antiqua" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender66" runat="server" TargetControlID="txt_month"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblorganization" runat="server" Text="Organization" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnorgnizationadd" runat="server" Visible="false" Text="+" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgnizationadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlorganization" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" OnSelectedIndexChanged="ddlorganization_OnSelectedIndexChanged"
                                                                    Width="200px" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Button ID="btnorganizationremove" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorganizationremove_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbldesignationexp" runat="server" Text="Designation" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btndesigadd" runat="server" Text="+" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btndesigadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddldesignationexp" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddldesignationexp_OnSelectedIndexChanged"
                                                                    Font-Names="Book Antiqua" Font-Bold="True" Width="200px" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btndesigremove" runat="server" Text="-" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btndesigremove_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblexpin" runat="server" Text="Experience in" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnexpinadd" runat="server" Text="+" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btnexpinadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlexpin" runat="server" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlexpin_OnSelectedIndexChanged"
                                                                    AutoPostBack="true" Font-Size="Medium" Font-Bold="True" Width="200px" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnexpinremove" runat="server" Text="-" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btnexpinremove_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblrforr" runat="server" Text="Reason for Relieving" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnrforradd" runat="server" Text="+" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btnrforradd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlrforr" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" OnSelectedIndexChanged="ddlrforr_OnSelectedIndexChanged"
                                                                    Width="200px" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnrforremove" runat="server" Text="-" Style="display: none;" Font-Names="Book Antiqua"
                                                                    Font-Size="Small" Height="21px" OnClick="btnrforremove_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel42" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <td>
                                                                        <asp:Label ID="lblsalary" runat="server" Text="Salary" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblslarystar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_salary" runat="server" MaxLength="10" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender67" runat="server" TargetControlID="txt_salary"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel43" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <td>
                                                                        <asp:Label ID="lbladditionalduties" runat="server" Text="Additional Duties" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lbladddutiesstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_addduties" runat="server" MaxLength="30" CssClass="textbox txtheight1 "
                                                                            TextMode="MultiLine" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                            Height="25px" Width="194px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender68" runat="server" TargetControlID="txt_addduties"
                                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:FileUpload ID="fucertificate" runat="server" />

                                                                <asp:Button ID="btnupload" runat="server" Text="Upload" Style="left: 815px; position: absolute; width: 68px;"
                                                                    OnClick="btnupload_Click" />
                                                                <asp:LinkButton ID="linkdownload" Text="DownloadAttachment" Font-Name="Book Antiqua" Font-Size="11pt"
                                                                    OnClick="lnkdownlaodattachement_Click" runat="server" Style="left: 890px; position: absolute; Width: 2px;" />
                                                            </td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnexperienceadd" runat="server" Text="Add" Font-Size="Medium" OnClick="btnexperienceadd_Click" />
                                                                <asp:Button ID="btnexperienceupdate" runat="server" Text="Update" Visible="false"
                                                                    Font-Size="Medium" OnClick="btnexperienceupdate_Click" />
                                                                <asp:Button ID="btn_edelete" runat="server" Text="Delete" Visible="false" Font-Size="Medium"
                                                                    OnClick="btn_edelete_Click" />
                                                                <asp:Button ID="btnexpexit" runat="server" Text="Exit" Font-Size="Medium" OnClick="btnexpexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblexpvalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelexp" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capexp" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_exp" Width="150px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender044" runat="server" TargetControlID="txt_exp"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                        <br />
                                        <asp:Button ID="btnexpadd1" Width="50px" runat="server" Text="Add" OnClick="btnexpadd1_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexpexit1" Width="50px" runat="server" Text="Exit" OnClick="btnexpexit1_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="colpaspanelex3" runat="server" TargetControlID="panelheaderexp"
                            CollapseControlID="panelexperiencecollaps" ExpandControlID="panelexperiencecollaps"
                            Collapsed="true" TextLabelID="lblfilter2" CollapsedSize="0" ImageControlID="imagefilter2"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelsemorgcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfilter3" Text="Seminar Attend" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefilter3" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheadersematt" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnsemattendadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnsemattendadd_Clcik"
                                                Style="margin-left: 800px;" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Seminar_grid" runat="server" Visible="false" OnRowDataBound="Seminar_grid_OnRowDataCommand"
                                                OnRowCommand="Seminar_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="SeminarAttend" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelsemattend" runat="server" BorderColor="Black" BackColor="White"
                                            Font-Bold="true" BorderWidth="2px" Height="280px" Width="430px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_sa" style="text-align: left; font-family: MS Sans Serif; font-size: medium; font-weight: bold">
                                                    <table style="width: 350px; margin-left: 25px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Label ID="lblheadersemdetails" runat="server" Style="color: #191970;" Text=" Seminors Details"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsemtype" runat="server" Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnseimtypeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnseimtypeadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlsemtype" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged=" ddlsemtype_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnsemtyperemove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnsemtyperemove_Click" />
                                                                <asp:Label ID="lblsemtypevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblprogramname" runat="server" Text="Program Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlprogramname" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged=" ddlprogramname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpnameremove_Click" />
                                                                <asp:Label ID="lblprogramnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsemfromdate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_semfromdate" Height="16px" Width="116px" OnTextChanged="txt_semfromdate_OnTextChanged"
                                                                    AutoPostBack="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_semfromdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender010" Format="dd/MM/yyyy" TargetControlID="txt_semfromdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsemtodate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_semtodate" Height="16px" Width="116px" OnTextChanged="txt_semtodate_OnTextChanged"
                                                                    AutoPostBack="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_semtodate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender011" Format="dd/MM/yyyy" TargetControlID="txt_semtodate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblplace" runat="server" Text="Place" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpaleceadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpaleceadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlplace" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged=" ddlplace_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpalceremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpalceremove_Click" />
                                                                <asp:Label ID="lblplacevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsponsored" runat="server" Text="Sponsored" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnspnsadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnspnsadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlsponsored" Width="200px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged=" ddlsponsored_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnsponsremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnsponsremove_Click" />
                                                                <asp:Label ID="lblsponsoredvalidatoin" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblstitle" runat="server" Text="Title" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnstitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnstitleadd_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlstile" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged=" ddlstile_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnstitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -30px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnstitleremove_Click" />
                                                                <asp:Label ID="lblstitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnsemattenadd1" runat="server" Text="Add" Font-Bold="True" OnClick="btnsemattenadd1_Click"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnsemattenupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    OnClick="btnsemattenupdate_Click" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btn_sadelete" runat="server" Text="Delete" Font-Bold="True" OnClick="btn_sadelete_Click"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnsemattenexit1" runat="server" Text="Exit" Font-Bold="True" OnClick="btnsemattenexit1_Click"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblsemattndvalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelsem" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capsem" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_semtype" Width="150px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender46" runat="server" TargetControlID="txt_semtype"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddsem" Width="50px" runat="server" Text="Add" OnClick="btnaddsem_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitsem" Width="50px" runat="server" Text="Exit" OnClick="btnexitsem_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="panelheadersematt"
                            CollapseControlID="panelsemorgcollaps" ExpandControlID="panelsemorgcollaps" Collapsed="true"
                            TextLabelID="lblfilter3" CollapsedSize="0" ImageControlID="imagefilter3" CollapsedImage="../images/right.jpeg"
                            ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelsemattndcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfilter4" Text="Seminar Orgnaized" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefilter4" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderorg" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnsemorgadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnsemorgadd_Click"
                                                Style="margin-left: 800px;" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="SeminarOrgs_grid" runat="server" Visible="false" OnRowDataBound="SeminarOrgs_grid_OnRowDataCommand"
                                                OnRowCommand="SeminarOrgs_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="SeminarOrgs" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelsemorg" runat="server" BorderColor="Black" BackColor="White"
                                            Font-Bold="true" BorderWidth="2px" Height="270px" Width="400px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_se" style="text-align: left; font-family: MS Sans Serif; font-size: Small; font-weight: bold">
                                                    <table style="margin-left: 50px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Label ID="lblheadsemorgnized" runat="server" Style="color: #191970;" Text="Seminor Organized"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblorgname" runat="server" Text="Program Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnorgnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlorgname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlorgname_OnSelectedIndexChanged"
                                                                    Style="margin-left: 2px;" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnorgnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgnameremove_Click" />
                                                                <asp:Label ID="lblorgnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblorgfromdate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_orgfromdate" AutoPostBack="true" OnTextChanged="txt_orgfromdate_OnTextChanged"
                                                                    Height="16px" Width="116px" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender03" runat="server" TargetControlID="txt_orgfromdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender012" Format="dd/MM/yyyy" TargetControlID="txt_orgfromdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblorgtodate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_orgtodate" AutoPostBack="true" OnTextChanged="txt_orgtodate_OnTextChanged"
                                                                    Height="16px" Width="116px" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender04" runat="server" TargetControlID="txt_orgtodate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender013" Format="dd/MM/yyyy" TargetControlID="txt_orgtodate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblorgplace" runat="server" Text="Place" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnorgplaceadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgplaceadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlorgplace" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlorgplace_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnorgplaceremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgplaceremove_Click" />
                                                                <asp:Label ID="lblorgplacevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblorgtitle" runat="server" Text="Title" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnorgtitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgtitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlorgtitle" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlorgtitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnorgtitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnorgtitleremove_Click" />
                                                                <asp:Label ID="lblorgtitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblletteref" runat="server" Text="Lette Reg. No" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_letteregno" Height="16px" Width="120" runat="server" MaxLength="15"
                                                                    CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="txt_letteregno"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnsemorgnizeadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnsemorgnizeadd_Click"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnsemorgnizeupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    OnClick="btnsemorgnizeupdate_Click" Visible="false" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" />
                                                                <asp:Button ID="btn_sodelete" runat="server" Text="Delete" Font-Bold="True" OnClick="btn_sodelete_Click"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnsemorgexit" runat="server" Text="Exit" Font-Bold="True" OnClick="btnsemorgexit_Click"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblorgvalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelo" runat="server" Visible="False" Style="width: 200px; height: 100px;"
                                    Font-Bold="true" BorderStyle="Solid" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="caporg" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_org" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender47" runat="server" TargetControlID="txt_org"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddorg" Width="50px" runat="server" Text="Add" OnClick="btnaddorg_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitorg" Width="50px" runat="server" Text="Exit" OnClick="btnexitorg_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="panelheaderorg"
                            CollapseControlID="panelsemattndcollaps" ExpandControlID="panelsemattndcollaps"
                            Collapsed="true" TextLabelID="lblfilter4" CollapsedSize="0" ImageControlID="imagefilter4"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="paneljournalcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfilterjournal" Text="Journal Publilcation" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagejournal" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderjournal" runat="server">
                            <center>
                                <table style="">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnjournaladd" runat="server" Text="Add" Style="margin-left: 800px;"
                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnjournaladd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="JournalPub1" runat="server" Visible="false" OnRowDataBound="JournalPub1_OnRowDataCommand"
                                                OnRowCommand="JournalPub1_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Journal" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="paneljournal" runat="server" BorderColor="Black" BackColor="White"
                                            Font-Bold="true" BorderWidth="2px" Height="520px" Width="400px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 78px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_j" style="text-align: left; font-family: MS Sans Serif; font-size: Medium; font-weight: bold">
                                                    <table style="margin-left: 60px;" >
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Label ID="lblheadjournal" runat="server" Style="color: #191970;" Text="Journal Publication"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbljtype" runat="server" Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnjtypeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjtypeadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddljtype" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddljtype_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnjtyperemove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjtyperemove_Click" />
                                                                <asp:Label ID="lbljtypevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbljname" runat="server" Text="Journal Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnjnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddljname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddljname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnjanmeremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjanmeremove_Click" />
                                                                <asp:Label ID="lbljnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpublisher" runat="server" Text="Publisher" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpublisheradd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpublisheradd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpublisher" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpublisher_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpublisherremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpublisherremove_Click" />
                                                                <asp:Label ID="lblpublishervalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel44" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_volumeno" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblvolumeno" runat="server" Text="Volume No" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblvolumenostar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_volumeno" runat="server" MaxLength="10" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender79" runat="server" TargetControlID="txt_volumeno"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />--%>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel45" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_issueno" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblissueno" runat="server" Text="Issue No" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblissuenosstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_issueno" runat="server" MaxLength="10" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <%--   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender80" runat="server" TargetControlID="txt_issueno"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />--%>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblperiodicity" runat="server" Text="Periodicity" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnperiodicityadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnperiodicityadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlperiodicity" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlperiodicity_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnperiodicityremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnperiodicityremove_Click" />
                                                                <asp:Label ID="lblpriodicityvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel46" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_preiod" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblpreiod" runat="server" Text="Period" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblpreiodstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_preiod" runat="server" MaxLength="15" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender94" runat="server" TargetControlID="txt_preiod"
                                                                            FilterType="Custom,uppercaseletters,lowercaseletters,numbers" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'''-" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel47" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_pageno" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblpageno" runat="server" Text="Page No" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblpagenostar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_pageno" runat="server" MaxLength="5" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender81" runat="server" TargetControlID="txt_pageno"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbljtitle" runat="server" Text="Title" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnjtitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjtitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddljtitle" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddljtitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnjtitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnjtitleremove_Click" />
                                                                <asp:Label ID="lbljtitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="LblAuthor" runat="server" Text="Author" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlAuthor" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAuthor_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="LblAuthor1" runat="server" Text="Author1" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                             <td></td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlAuthor1" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAuthor1_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="LblAuthor2" runat="server" Text="Author2" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlAuthor2" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAuthor2_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="LblAuthor3" runat="server" Text="Author3" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlAuthor3" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAuthor3_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnjournapublicationladd" runat="server" Text="Add" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnjournapublicationladd_Click" />
                                                                <asp:Button ID="btnjournapublicationlupdate" runat="server" Text="Update" OnClick="btnjournapublicationlupdate_Click"
                                                                    Font-Bold="true" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btn_jdelete" runat="server" Text="Delete" OnClick="btn_jdelete_Click"
                                                                    Font-Bold="true" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnjournalexit" runat="server" Text="Exit" Font-Size="Medium" OnClick="btnjournalexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblvalidationjournalpublication" runat="server" Text="" Visible="false"
                                                                    Font-Bold="true" ForeColor="Red" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelj" runat="server" Visible="False" Style="width: 200px; height: 100px;"
                                    BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capj" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_j" Width="100px" CssClass="textbox txtheight1 " Height="14px"
                                            runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender48" runat="server" TargetControlID="txt_j"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnjadd" Width="50px" runat="server" Text="Add" OnClick="btnjadd_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        <asp:Button ID="btnjexit" Width="50px" runat="server" Text="Exit" OnClick="btnjexit_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="panelheaderjournal"
                            CollapseControlID="paneljournalcollaps" ExpandControlID="paneljournalcollaps"
                            Collapsed="true" TextLabelID="lblfilterjournal" CollapsedSize="0" ImageControlID="imagejournal"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelbookscollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblbooks" Text="Book Published" runat="server" Font-Size="Large" Font-Bold="True"
                                    Font-Names="Book Antiqua" />
                                <asp:Image ID="imagebooks" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderbook" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnbooksadd" runat="server" Text="Add" Font-Bold="True" OnClick="btnbooksadd_Click"
                                                Style="margin-left: 800px;" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Book_grid" runat="server" Visible="false" OnRowDataBound="Book_grid_OnRowDataCommand"
                                                OnRowCommand="Book_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Book" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelbooks" runat="server" BorderColor="Black" BackColor="White" Font-Bold="true"
                                            BorderWidth="2px" Height="250px" Width="500px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_b" style="text-align: left; font-family: Book Antiqua; font-weight: bold; font-size: Small; font-weight: bold">
                                                    <table style="margin-left: 50px;">
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:Label ID="lblheadbook" runat="server" Style="color: #191970;" Text="Books Published"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblbyear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_byear" CssClass="textbox txtheight1 " runat="server" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender05" runat="server" TargetControlID="txt_byear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender24" Format="dd/MM/yyyy" TargetControlID="txt_byear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblbpname" runat="server" Text="Publisher Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnbpnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnbpnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlbpname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlbpname_OnSelectedIndexChanged"
                                                                    Width="150px" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnbpnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnbpnameremove_Click" />
                                                                <asp:Label ID="lblbpnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_bpaddress" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblbpaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblbpaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_bpaddress" runat="server" MaxLength="100" TextMode="MultiLine"
                                                                            CssClass="textbox txtheight1 " Font-Names="Book Antiqua" Font-Bold="true" Width="250px"
                                                                            Font-Size="Medium"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender88" runat="server" TargetControlID="txt_bpaddress"
                                                                            FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblbptitle" runat="server" Text="Title" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnbptitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnbptitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlbptitle" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlbptitle_OnSelectedIndexChanged"
                                                                    Width="150px" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnbptitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnbptitleremove_Click" />
                                                                <asp:Label ID="lblbptitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel49" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_costofbook" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblcostofbook" runat="server" Text="Cost of Book" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblcostofbookstar" runat="server" Visible="false" ForeColor="Red"
                                                                            Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_costofbook" runat="server" MaxLength="8" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender75" runat="server" TargetControlID="txt_costofbook"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:Button ID="btnbookspublishedadd" runat="server" Text="Add" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnbookspublishedadd_Click" />
                                                                <asp:Button ID="btnbookspublishedupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnbookspublishedupdate_Click" />
                                                                <asp:Button ID="btn_bkdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_bkdelete_Click" />
                                                                <asp:Button ID="btnbooksexit" runat="server" Text="Exit" Font-Size="Medium" OnClick="btnbooksexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblvalidationbook" runat="server" Text="" Visible="false" ForeColor="Red"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelb" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Bold="true" Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capb" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_b" Width="150px" Height="16px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender49" runat="server" TargetControlID="txt_b"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnbadd" Width="50px" runat="server" Text="Add" OnClick="btnbadd_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnbexit" Width="50px" runat="server" Text="Exit" OnClick="btnbexit_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="panelheaderbook"
                            CollapseControlID="panelbookscollaps" ExpandControlID="panelbookscollaps" Collapsed="true"
                            TextLabelID="lblbooks" CollapsedSize="0" ImageControlID="imagebooks" CollapsedImage="../images/right.jpeg"
                            ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="paneleditorbooks" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lbleditorbook" Text="Editor Books Details" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imageeditorbook" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderedit" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btneditoradd" runat="server" Text="Add" Style="margin-left: 800px;"
                                                Font-Size="Medium" OnClick="btneditoradd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="EditorBook_grid" runat="server" Visible="false" OnRowDataBound="EditorBook_grid_OnRowDataCommand"
                                                OnRowCommand="EditorBook_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Editor" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="paneledit" runat="server" BorderColor="Black" BackColor="White" BorderWidth="2px"
                                            Height="290px" Width="450px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_e" style="text-align: left; font-family: MS Sans Serif; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Label ID="lblheadbookedit" runat="server" Style="color: #191970;" Text="Editor Books / Journal Details"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbletype" runat="server" Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnetypeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnetypeadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddletype" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddletype_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnetyperemove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnetyperemove_Click" />
                                                                <asp:Label ID="lbletypevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbleyear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_eyear" Height="16px" Width="116px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender220" runat="server" TargetControlID="txt_eyear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender001" Format="dd/MM/yyyy" TargetControlID="txt_eyear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblepname" runat="server" Text="Publisher Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnepnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnepnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlepname" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlepname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnepnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnepnameremove_Click" />
                                                                <asp:Label ID="lblepnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel50" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_eaddress" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lbladdress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lbleaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_eaddress" runat="server" CssClass="textbox txtheight1 " MaxLength="100"
                                                                            TextMode="MultiLine" Font-Names="Book Antiqua" Font-Bold="true" Width="250px"
                                                                            Font-Size="Medium"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender89" runat="server" TargetControlID="txt_eaddress"
                                                                            FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lbletitle" runat="server" Text="Title" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnetitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnetitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddletitle" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddletitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnetitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnetitleremove_Click" />
                                                                <asp:Label ID="lbletitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel51" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_ecostofbook" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblecostofbook" runat="server" Text="Cost of Book" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblecostbookstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_ecostofbook" runat="server" MaxLength="10" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender82" runat="server" TargetControlID="txt_ecostofbook"
                                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btneditorbooksadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btneditorbooksadd_Click" />
                                                                <asp:Button ID="btneditorbooksupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btneditorbooksupdate_Click" />
                                                                <asp:Button ID="btn_ebkdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_ebkdelete_Click" />
                                                                <asp:Button ID="btneditorbooksexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btneditorbooksexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lbledivalidation" runat="server" Text="" Visible="false" ForeColor="Red"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panele" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="cape" style="height: 16px; top: 16px; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_e" Width="150px" Height="16px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender50" runat="server" TargetControlID="txt_e"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnadde" Width="50px" runat="server" Text="Add" OnClick="btnadde_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexite" Width="50px" runat="server" Text="Exit" OnClick="btnexite_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="panelheaderedit"
                            CollapseControlID="paneleditorbooks" ExpandControlID="paneleditorbooks" Collapsed="true"
                            TextLabelID="lbleditorbook" CollapsedSize="0" ImageControlID="imageeditorbook"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelaward" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblawardcolaps" Text="Award" runat="server" Font-Size="Large" Font-Bold="true"
                                    Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefiltercollpas" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheadaward" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btnawardadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnawardadd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblheaderaward" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Award_grid" runat="server" Visible="false" OnRowDataBound="Award_grid_OnRowDataCommand"
                                                OnRowCommand="Award_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Award" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelhdaward" runat="server" BorderColor="Black" BackColor="White"
                                            Font-Bold="true" BorderWidth="2px" Height="250px" Width="480px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="div_ap" style="text-align: left; font-family: MS Sans Serif; font-size: Small; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:Label ID="lblheadaward" runat="server" Style="color: #191970;" Text="Award Details"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblayear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_ayear" Height="16px" Width="116px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender01" runat="server" TargetControlID="txt_ayear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" TargetControlID="txt_ayear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblaname" runat="server" Text="Name" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnanameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnanameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlaname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlaname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnanameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnanameremove_Click" />
                                                                <asp:Label ID="lblanamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblaoname" runat="server" Text="Organization Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnaoanameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnaoanameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlaoaname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlaoaname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnaoanameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnaoanameremove_Click" />
                                                                <asp:Label ID="lblaonamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblaaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblaaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_aaddress" runat="server" Width="250px" CssClass="textbox txtheight1 "
                                                                    MaxLength="100" TextMode="MultiLine" Font-Names="Book Antiqua" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender90" runat="server" TargetControlID="txt_aaddress"
                                                                    FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblanaward" runat="server" Text="Nature of Award" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnanawardadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnanawardadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlanaward" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlanaward_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnanawardremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnanawardremove_Click" />
                                                                <asp:Label ID="lblanawardvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnawarddetailsadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnawarddetailsadd_Click" />
                                                                <asp:Button ID="btnawarddetailsupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnawarddetailsupdate_Click" />
                                                                <asp:Button ID="btn_awdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_awdelete_Click" />
                                                                <asp:Button ID="btnawardexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnawardexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:Label ID="lblawardvaidation" runat="server" Text="" Visible="false" ForeColor="Red"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panela" runat="server" Visible="False" Style="width: 200px; font-weight: bold; height: 70px;"
                                    BorderStyle="Solid" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capa" style="height: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_a" Width="100px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" runat="server" TargetControlID="txt_a"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnadda" Width="50px" runat="server" Text="Add" OnClick="btnadda_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexita" Width="50px" runat="server" Text="Exit" OnClick="btnexita_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="panelheadaward"
                            CollapseControlID="panelaward" ExpandControlID="panelaward" Collapsed="true"
                            TextLabelID="lblawardcolaps" CollapsedSize="0" ImageControlID="imagefiltercollpas"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelprojectcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblfiltercolaps" Text="Project Details" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagefiltercolaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheadproject" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btnrpojectadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnrpojectadd_Clcik" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="project_detail_grid" runat="server" Visible="false" OnRowDataBound="project_detail_grid_OnRowDataCommand"
                                                OnRowCommand="project_detail_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Project" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelproject" runat="server" BorderColor="Black" BackColor="White"
                                            BorderWidth="2px" Font-Bold="true" Height="200px" Width="400px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="Div12" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Project Details</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblprojecttitle" runat="server" Text="Title" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnprojecttitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprojecttitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlprojecttitle" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlprojecttitle_OnSelectedIndexChanged"
                                                                    Style="margin-left: -105px;" CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnprojecttitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprojecttitleremove_Click" />
                                                                <asp:Label ID="lblprojecttitlevalidation" runat="server" ForeColor="Red" Text=""
                                                                    Font-Names="Book Antiqua" Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblprojectdate" runat="server" Text="From" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_projectfrom" AutoPostBack="true" OnTextChanged="txt_projectfrom_OnTextChanged"
                                                                    Height="16px" Width="90" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender172" runat="server" TargetControlID="txt_projectfrom"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender117" Format="dd/MM/yyyy" TargetControlID="txt_projectfrom"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblprojecttodate" runat="server" Text="To" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                                <asp:TextBox ID="txt_projecto" AutoPostBack="true" OnTextChanged="txt_projecto_OnTextChanged"
                                                                    Height="16px" Width="90" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender99" runat="server" TargetControlID="txt_projecto"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender118" Format="dd/MM/yyyy" TargetControlID="txt_projecto"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                                <asp:Label ID="lblprojectdatevalidation" runat="server" ForeColor="Red" Text="Enter from Date first"
                                                                    Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="Label5" runat="server" Text="Remarks" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="Label6" runat="server" Visible="false" ForeColor="Red" Text="*" Font-Size="Medium"
                                                                    Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_prmks" runat="server" MaxLength="20" Width="218px" CssClass="textbox txtheight1 "
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Style="margin-left: -103px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender83" runat="server" TargetControlID="txt_prmks"
                                                                    FilterType="Custom,uppercaseletters,lowercaseletters" ValidChars="letters">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnprojectdetailsadd" runat="server" Text="Add" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprojectdetailsadd_Click" />
                                                                <asp:Button ID="btnprojectdetailsupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprojectdetailsupdate_Click" />
                                                                <asp:Button ID="btn_pddelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_pddelete_Click" />
                                                                <asp:Button ID="btnprojectdetaisexit" runat="server" Text="Exit" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprojectdetaisexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblprojectvalidation" runat="server" Text="" ForeColor="Red" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelp" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capp" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_p" Width="150px" Height="14px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" TargetControlID="txt_p"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddp" Width="50px" runat="server" Text="Add" OnClick="btnaddp_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitp" Width="50px" runat="server" Text="Exit" OnClick="btnexitp_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="panelheadproject"
                            CollapseControlID="panelprojectcollaps" ExpandControlID="panelprojectcollaps"
                            Collapsed="true" TextLabelID="lblfiltercolaps" CollapsedSize="0" ImageControlID="imagefiltercolaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelresearchcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblresearchcollaps" Text="Research Details" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imageresearchcollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheadresearch" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnresearchdetails" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnresearchdetails_Clcik" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Research_grid" runat="server" Visible="false" OnRowDataBound="Research_grid_OnRowDataCommand"
                                                OnRowCommand="Research_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Research" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelresearch" runat="server" BorderColor="Black" BackColor="White"
                                            BorderWidth="2px" Font-Bold="true" Height="230px" Width="420px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="divresearch" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 30px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Research Details</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblresearchname" runat="server" Text="Name" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnresearchnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnresearchnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlresearchname" Width="200px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlresearchname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnresearchnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnresearchnameremove_Click" />
                                                                <asp:Label ID="lblrenamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblresearchtype" runat="server" Text="Type" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnresearchtypeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnresearchtypeadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlresearchtype" Width="200px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlresearchtype_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnresearchtyperemove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnresearchtyperemove_Click" />
                                                                <asp:Label ID="lblretypevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblrrtype" runat="server" Text="Research Type" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnrrtypeadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnrrtypeadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlrrtype" Width="200px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlrrtype_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnrryperemvoe" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnrryperemvoe_Click" />
                                                                <asp:Label ID="lblrrtypevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel54" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_rremarks" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <td class="style5">
                                                                        <asp:Label ID="lblrremarks" runat="server" Text="Remarks" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td class="style55"></td>
                                                                    <td class="style25">
                                                                        <asp:Label ID="lblrremarksstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                            Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                        <asp:TextBox ID="txt_rremarks" runat="server" MaxLength="20" Width="190px" CssClass="textbox txtheight1 "
                                                                            Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender84" runat="server" TargetControlID="txt_rremarks"
                                                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnresearachdetailsadd" runat="server" Text="Add" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnresearachdetailsadd_Click" />
                                                                <asp:Button ID="btnresearchdetailsupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnresearchdetailsupdate_Click" />
                                                                <asp:Button ID="btn_rddelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_rddelete_Click" />
                                                                <asp:Button ID="btnresearchexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnresearchexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblresearchvalidation" runat="server" Text="" Visible="false" ForeColor="Red"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelr" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capr" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_r" CssClass="textbox txtheight1 " runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender53" runat="server" TargetControlID="txt_r"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddr" Width="50px" runat="server" Text="Add" OnClick="btnaddr_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitr" Width="50px" runat="server" Text="Exit" OnClick="btnexitr_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender8" runat="server" TargetControlID="panelheadresearch"
                            CollapseControlID="panelresearchcollaps" ExpandControlID="panelresearchcollaps"
                            Collapsed="true" TextLabelID="lblresearchcollaps" CollapsedSize="0" ImageControlID="imageresearchcollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelcollapspg" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblpgcollaps" Text="Project Grants" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagecollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderpg" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btngrantsadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngrantsadd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="PGrants_grid" runat="server" Visible="false" OnRowDataBound="PGrants_grid_OnRowDataCommand"
                                                OnRowCommand="PGrants_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Grants" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelprojectgrants" runat="server" BorderColor="Black" BackColor="White"
                                            BorderWidth="2px" Font-Bold="true" Height="300px" Width="440px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="projectgrantdiv" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 50px;">
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Project Grants</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgyear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpgyearadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgyearadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpgyear" runat="server" Width="175px" Visible="false" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpgyear_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_pgyear" CssClass="textbox txtheight1 " Width="125px" runat="server"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender95" runat="server" TargetControlID="txt_pgyear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender3" Format="dd/MM/yyyy" TargetControlID="txt_pgyear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpgyearremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgyearremove_Click" />
                                                                <asp:Label ID="lblpgyearvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgname" runat="server" Text="Name" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpgnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpgname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpgname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpgnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgnameremove_Click" />
                                                                <asp:Label ID="lblpgnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpgaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pgaddress" runat="server" MaxLength="100" TextMode="MultiLine"
                                                                    CssClass="textbox txtheight1 " Font-Names="Book Antiqua" Font-Bold="true" Width="250px"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender91" runat="server" TargetControlID="txt_pgaddress"
                                                                    FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgtitle" runat="server" Text="Title" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpgtitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgtitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpgtitle" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpgtitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpgtitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpgtitleremove_Click" />
                                                                <asp:Label ID="lblpgtitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgamount" runat="server" Text="Amount" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpgamountstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pgamount" runat="server" MaxLength="10" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" Width="125px" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender76" runat="server" TargetControlID="txt_pgamount"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpgletterno" runat="server" Text="Letter No" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpgletternostat" runat="server" Visible="false" ForeColor="Red"
                                                                    Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pgletterno" runat="server" MaxLength="15" CssClass="textbox txtheight1 "
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Width="125px" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender77" runat="server" TargetControlID="txt_pgletterno"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblpgdate" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_pgdate" CssClass="textbox txtheight1 " Width="125px" runat="server"
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender151" runat="server" TargetControlID="txt_pgdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender155" Format="dd/MM/yyyy" TargetControlID="txt_pgdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnprojectgrantsadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnprojectgrantsadd_Click" />
                                                                <asp:Button ID="btnprojectgrantsupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprojectgrantsupdate_Click" />
                                                                <asp:Button ID="btn_pgdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_pgdelete_Click" />
                                                                <asp:Button ID="btnprojectgrantsexit" runat="server" Text="Exit" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprojectgrantsexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblprojectgrantsvalidation" runat="server" Text="" Visible="false"
                                                                    ForeColor="Red" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelpg" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="cappg" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_pg" CssClass="textbox txtheight1 " runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender54" runat="server" TargetControlID="txt_pg"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddpg" Width="50px" runat="server" Text="Add" OnClick="btnaddpg_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitpg" Width="50px" runat="server" Text="Exit" OnClick="btnexitpg_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender9" runat="server" TargetControlID="panelheaderpg"
                            CollapseControlID="panelcollapspg" ExpandControlID="panelcollapspg" Collapsed="true"
                            TextLabelID="lblpgcollaps" CollapsedSize="0" ImageControlID="imagecollaps" CollapsedImage="../images/right.jpeg"
                            ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelpatencollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblpatencollaps" Text="Patent Received" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagepatencollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderpatent" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btnpatentadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnpatentadd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Patent_grid" runat="server" Visible="false" OnRowDataBound="Patent_grid_OnRowDataCommand"
                                                OnRowCommand="Patent_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Patent" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelpatent" runat="server" BorderColor="Black" BackColor="White"
                                            BorderWidth="2px" Font-Bold="true" Height="350px" Width="460px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="Div13" style="text-align: left; font-weight: bold; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Patent Received</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpryear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpryearadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpryearadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpryear" runat="server" Visible="false" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlpryear_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_pryear" Height="16px" Width="125px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender96" runat="server" TargetControlID="txt_pryear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender4" Format="dd/MM/yyyy" TargetControlID="txt_pryear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpryearremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpryearremove_Click" />
                                                                <asp:Label ID="lblpryearvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblprname" runat="server" Text="Name" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnprnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlprname" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlprname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnprnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprnameremove_Click" />
                                                                <asp:Label ID="lblprnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpraddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpraddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_praddress" runat="server" Width="250px" MaxLength="300" TextMode="MultiLine"
                                                                    Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender92" runat="server" TargetControlID="txt_praddress"
                                                                    FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblprtitle" runat="server" Text="Title" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnprtitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprtitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlprtitle" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlprtitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnprtitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnprtitleremove_Click" />
                                                                <asp:Label ID="lblprtitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblprletterno" runat="server" Text="Letter No" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblprletternostar" runat="server" Visible="false" ForeColor="Red"
                                                                    Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_prletterno" runat="server" Width="125px" MaxLength="15" CssClass="textbox txtheight1 "
                                                                    Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender85" runat="server" TargetControlID="txt_prletterno"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblprdate" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_prdate" Height="16px" Width="125px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender122" runat="server" TargetControlID="txt_prdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender16" Format="dd/MM/yyyy" TargetControlID="txt_prdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblprcerno" runat="server" Text="Certificate No" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblprcernostar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_prcerno" runat="server" Width="125px" MaxLength="15" CssClass="textbox txtheight1 "
                                                                    Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender86" runat="server" TargetControlID="txt_prcerno"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblprcerdate" runat="server" Text="Certificate Date" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txt_prcerdate" Height="16px" Width="125px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender93" runat="server" TargetControlID="txt_prcerdate"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender124" Format="dd/MM/yyyy" TargetControlID="txt_prcerdate"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnpatentreceivedadd" runat="server" Text="Add" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnpatentreceivedadd_Click" />
                                                                <asp:Button ID="btnpatentreceivedupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnpatentreceivedupdate_Click" />
                                                                <asp:Button ID="btn_ptdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_ptdelete_Click" />
                                                                <asp:Button ID="btnpatentexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnpatentexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblpatentvalidation" runat="server" Visible="false" ForeColor="Red"
                                                                    Text="" Font-Size="Medium" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelpr" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="cappr" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_pr" CssClass="textbox txtheight1 " runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_pr"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                        <br />
                                        <asp:Button ID="btnaddpr" Width="50px" runat="server" Text="Add" OnClick="btnaddpr_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitpr" Width="50px" runat="server" Text="Exit" OnClick="btnexitpr_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender10" runat="server" TargetControlID="panelheaderpatent"
                            CollapseControlID="panelpatencollaps" ExpandControlID="panelpatencollaps" Collapsed="true"
                            TextLabelID="lblpatencollaps" CollapsedSize="0" ImageControlID="imagepatencollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelguidecollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblcollapsguide" Text="Guide ship" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagecollapsguide" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderguide" runat="server">
                            <center>
                                <table>
                                    <tr align="right">
                                        <td>
                                            <asp:Button ID="btnguideadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnguideadd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Guide_grid" runat="server" Visible="false" OnRowDataBound="Guide_grid_OnRowDataCommand"
                                                OnRowCommand="Guide_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Guide" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelguide" runat="server" BorderColor="Black" BackColor="White" BorderWidth="2px"
                                            Font-Bold="true" Height="200px" Width="375px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="Div14" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Guide Ship</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblgyear" runat="server" Text="Year" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btngyearadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngyearadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlgyear" runat="server" Visible="false" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlgyear_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_gyear" Height="16px" Width="125px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender97" runat="server" TargetControlID="txt_gyear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender5" Format="dd/MM/yyyy" TargetControlID="txt_gyear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btngyearremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngyearremove_Click" />
                                                                <asp:Label ID="lblgyearvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblnatureguide" runat="server" Text="Nature of Guide" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnnatureguideadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnnatureguideadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlnatureguide" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlnatureguide_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnnatureguideremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnnatureguideremove_Click" />
                                                                <asp:Label ID="lblnatureguidevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblgtitle" runat="server" Text="Title" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btngtitleadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngtitleadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlgtitle" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlgtitle_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btngtitleremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btngtitleremove_Click" />
                                                                <asp:Label ID="lblgtitlevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnguideshipadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnguideshipadd_Click" />
                                                                <asp:Button ID="btnguideshipupdate" runat="server" Text="Update" Font-Bold="True"
                                                                    Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnguideshipupdate_Click" />
                                                                <asp:Button ID="btn_gddelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_gddelete_Click" />
                                                                <asp:Button ID="btngudieshipexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btngudieshipexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblgudievalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelgu" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capgu" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_gu" Width="150px" Height="16px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_gu"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                        <br />
                                        <asp:Button ID="btnaddgu" Width="50px" runat="server" Text="Add" OnClick="btnaddgu_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitgu" Width="50px" runat="server" Text="Exit" OnClick="btnexitgu_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender11" runat="server" TargetControlID="panelheaderguide"
                            CollapseControlID="panelguidecollaps" ExpandControlID="panelguidecollaps" Collapsed="true"
                            TextLabelID="lblcollapsguide" CollapsedSize="0" ImageControlID="imagecollapsguide"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelpmcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblpmcollaps" Text="Professional Membership" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imagepmcollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderpm" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnprofadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnprofadd_Clcik" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Professionalm_grid" runat="server" Visible="false" OnRowDataBound="Professionalm_grid_OnRowDataCommand"
                                                OnRowCommand="Professionalm_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Professional" runat="server" visible="false" class="popupstyle popupheight1"
                                    style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                    <center>
                                        <asp:Panel ID="panelpm" runat="server" BorderColor="Black" BackColor="White" BorderWidth="2px"
                                            Font-Bold="true" Height="220px" Width="460px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                            <center>
                                                <div class="PopupHeaderrstud2" id="Div15" style="text-align: left; font-weight: bold; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                    <table style="margin-left: 40px;">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <caption style="color: #191970; font-weight: bold;">
                                                                    Professional Membership</caption>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpmname" runat="server" Text="Name" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpmnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpmnameadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpmname" Width="175px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpmname_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpmnameremove" runat="server" Text="-" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpmnameremove_Click" />
                                                                <asp:Label ID="lblpmnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpmaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpmaddressstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pmaddress" runat="server" Width="250px" MaxLength="100" TextMode="MultiLine"
                                                                    Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender053" runat="server" TargetControlID="txt_pmaddress"
                                                                    FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpmjonidyear" runat="server" Text="Joined Year" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnpmjoindyearadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpmjoindyearadd_Click" />
                                                            </td>
                                                            <td class="style25">
                                                                <asp:DropDownList ID="ddlpmjoindyear" runat="server" Visible="false" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpmjoindyear_OnSelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_jyear" Height="16px" Width="125px" runat="server" CssClass="textbox txtheight1 "
                                                                    Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender98" runat="server" TargetControlID="txt_jyear"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                                <asp:CalendarExtender ID="CalendarExtender19" Format="dd/MM/yyyy" TargetControlID="txt_jyear"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td class="style27">
                                                                <asp:Button ID="btnpmjoindyearremove" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                    OnClick="btnpmjoindyearremove_Click" />
                                                                <asp:Label ID="lblpmjoindyearvalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                    Visible="false" Font-Size="5pt"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                <asp:Label ID="lblpmno" runat="server" Text="Membership No" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td class="style55"></td>
                                                            <td class="style25">
                                                                <asp:Label ID="lblpmstar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                    Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                                <asp:TextBox ID="txt_pmno" runat="server" MaxLength="15" Font-Names="Book Antiqua"
                                                                    Font-Bold="true" Width="125px" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender87" runat="server" TargetControlID="txt_pmno"
                                                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button ID="btnpmemberadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnpmemberadd_Click" />
                                                                <asp:Button ID="btnpmemberupdate" runat="server" Text="Update" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnpmemberupdate_Click" />
                                                                <asp:Button ID="btn_mmdelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_mmdelete_Click" />
                                                                <asp:Button ID="btnpmemberexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" OnClick="btnpmemberexit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblprofvalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                    ForeColor="Red" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <asp:Panel ID="panelprof" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capprof" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_prof" CssClass="textbox txtheight1 " runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender145" runat="server" TargetControlID="txt_prof"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                        <br />
                                        <asp:Button ID="btnaddprof" Width="50px" runat="server" Text="Add" OnClick="btnaddprof_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitprof" Width="50px" runat="server" Text="Exit" OnClick="btnexitprof_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender12" runat="server" TargetControlID="panelheaderpm"
                            CollapseControlID="panelpmcollaps" ExpandControlID="panelpmcollaps" Collapsed="true"
                            TextLabelID="lblpmcollaps" CollapsedSize="0" ImageControlID="imagepmcollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <div>
                            <asp:Panel ID="panelacmcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblacmcollaps" Text="Administrative Committee Member" runat="server"
                                    Font-Size="Large" Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imageacmcollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderacm" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnadminadd" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnadminadd_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Administrative_grid" runat="server" Visible="false" OnRowDataBound="Administrative_grid_OnRowDataCommand"
                                                OnRowCommand="Administrative_grid_OnRowCommand" AutoGenerateColumns="true" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <div id="Administrative" runat="server" visible="false" class="popupstyle popupheight1"
                                style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                <center>
                                    <asp:Panel ID="panelamc" runat="server" BorderColor="Black" BackColor="White" BorderWidth="2px"
                                        Height="250px" Width="400px" Style="background-color: #ffccff; border-radius: 10px; margin-top: 175px;">
                                        <div class="PopupHeaderrstud2" id="Div16" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:Label ID="lblheaderadmincommitee" runat="server" Style="color: #191970;" Text="Administrative Committee Member"
                                                                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblamname" runat="server" Text="Committee Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnamnameadd" runat="server" Text="+" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                OnClick="btnamnameadd_Click" />
                                                        </td>
                                                        <td class="style25">
                                                            <asp:DropDownList ID="ddlamname" runat="server" Width="175px" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlamname_OnSelectedIndexChanged"
                                                                CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="style27">
                                                            <asp:Button ID="btnamnameremove" runat="server" Visible="false" Text="-" Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 0px; width: 25px; background-color: #6699ee; border-radius: 6px;"
                                                                OnClick="btnamnameremove_Click" />
                                                            <asp:Label ID="lblamnamevalidation" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" Font-Size="5pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblamfromdate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:TextBox ID="txt_amfromdate" Height="16px" AutoPostBack="true" OnTextChanged="txt_amfromdate_OnTextChanged"
                                                                Width="125px" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender143" runat="server" TargetControlID="txt_amfromdate"
                                                                FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            <asp:CalendarExtender ID="CalendarExtender27" Format="dd/MM/yyyy" TargetControlID="txt_amfromdate"
                                                                runat="server">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblamtodate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:TextBox ID="txt_amtodate" Height="16px" AutoPostBack="true" OnTextChanged="txt_amtodate_OnTextChanged"
                                                                Width="125px" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_amtodate"
                                                                FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                            <asp:CalendarExtender ID="CalendarExtender28" Format="dd/MM/yyyy" TargetControlID="txt_amtodate"
                                                                runat="server">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblnatureofduty" runat="server" Text="Nature of Duty" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style55"></td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblndutystar" runat="server" Visible="false" ForeColor="Red" Text="*"
                                                                Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_natureofduty" runat="server" MaxLength="20" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Width="125px" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender74" runat="server" TargetControlID="txt_natureofduty"
                                                                FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style5">
                                                            <asp:Label ID="lblachievements" runat="server" Text="Achievements" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td class="style55"></td>
                                                        <td class="style25">
                                                            <asp:Label ID="lblachievementsstar" runat="server" Visible="false" ForeColor="Red"
                                                                Text="*" Font-Size="Medium" Font-Bold="false"></asp:Label>
                                                            <asp:TextBox ID="txt_achievements" runat="server" MaxLength="20" Font-Names="Book Antiqua"
                                                                Font-Bold="true" Width="125px" Font-Size="Medium" CssClass="textbox txtheight1 "></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:Button ID="btnamcommitteeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" OnClick="btnamcommitteeadd_Click" />
                                                            <asp:Button ID="btnamcommitteeupdate" runat="server" Text="Update" Font-Bold="True"
                                                                Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnamcommitteeupdate_Click" />
                                                            <asp:Button ID="btn_admindelete" runat="server" Text="Delete" Font-Bold="True" Visible="false"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_admindelete_Click" />
                                                            <asp:Button ID="btnamcommiteeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" OnClick="btnamcommiteeexit_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <asp:Label ID="lblcommitteevalidation" runat="server" Text="" Font-Names="Book Antiqua"
                                                                Visible="false" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </asp:Panel>
                                </center>
                            </div>
                            <br />
                            <center>
                                <asp:Panel ID="panelacm" runat="server" Visible="False" Style="width: 200px; height: 70px;"
                                    BorderStyle="Solid" Font-Bold="true" BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <center>
                                        <caption runat="server" id="capacm" style="height: 16px; top: 16px; font-weight: bold; font-variant: Medium-caps">
                                        </caption>
                                        <br />
                                        <asp:TextBox ID="txt_acm" Width="150px" Height="16px" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_acm"
                                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                        <br />
                                        <asp:Button ID="btnaddacm" Width="50px" runat="server" Text="Add" OnClick="btnaddacm_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                        &nbsp;
                                        <asp:Button ID="btnexitacm" Width="50px" runat="server" Text="Exit" OnClick="btnexitacm_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                    </center>
                                </asp:Panel>
                            </center>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender13" runat="server" TargetControlID="panelheaderacm"
                            CollapseControlID="panelacmcollaps" ExpandControlID="panelacmcollaps" Collapsed="true"
                            TextLabelID="lblacmcollaps" CollapsedSize="0" ImageControlID="imageacmcollaps"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />

                        <div>
                            <asp:Panel ID="pnlother" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                                Width="1034px">
                                <asp:Label ID="lblother" Text="Other Certificates" runat="server" Font-Size="Large"
                                    Font-Bold="true" Font-Names="Book Antiqua" />
                                <asp:Image ID="imageother" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="panelheaderother" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnother" runat="server" Text="Add" Font-Bold="True" Style="margin-left: 800px;"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnother_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="grdothers" runat="server" Visible="false" OnRowDataBound="grdothers_OnRowDataCommand"
                                                OnRowCommand="grdothers_OnRowCommand" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" GridLines="Both"
                                                HeaderStyle-BackColor="#3399ff" HeaderStyle-ForeColor="White">


                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_Sig_No" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CertificateName" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_certname" runat="server" Text='<%#Eval("CertificateName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CertificateNumber" HeaderStyle-BackColor="#3399ff" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_certnum" runat="server" Text='<%#Eval("CertificateNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>

                                                </Columns>


                                            </asp:GridView>

                                        </td>
                                    </tr>
                                </table>
                            </center>

                            <div id="otherdiv" runat="server" visible="false" class="popupstyle popupheight1"
                                style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                <center>
                                    <asp:Panel ID="panel10" runat="server" BorderColor="Black" BackColor="White"
                                        BorderWidth="2px" Style="background-color: #ffccff; border-radius: 10px; background-color: #ffccff; margin-top: 175px;"
                                        Height="400px" Width="600px">
                                        <center>
                                            <div class="PopupHeaderrstud2" id="div23" style="text-align: left; font-family: Book Antiqua; font-size: medium; font-weight: bold">
                                                <table style="margin-left: 40px;">
                                                    <caption style="color: #191970; font-weight: bold;">
                                                        Other Certificate</caption>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcertificatename" runat="server" Text="Certificate Name" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcertificatename"
                                                                Height="16px" Width="240" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>


                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcertificatenum" runat="server" Text="Certificate Number" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcertificatenum"
                                                                Height="16px" Width="240" runat="server" CssClass="textbox txtheight1 " Font-Size="Medium"></asp:TextBox>


                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td colspan="4" align="center">
                                                            <asp:FileUpload ID="FileUploadother" runat="server" />

                                                            <asp:Button ID="btnothercertupload" runat="server" Text="Upload" Style="left: 815px; position: absolute; width: 68px;"
                                                                OnClick="othercertupload_Click" />
                                                            <asp:LinkButton ID="Lnkbtnothercertupload" Text="DownloadAttachment" Font-Name="Book Antiqua" Font-Size="11pt"
                                                                OnClick="btnothercertupload_Click" runat="server" Style="left: 890px; position: absolute; Width: 2px;" />
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:Button ID="btnotherssave" runat="server" Text="Add" Font-Size="Medium" OnClick="btnotherssave_Click" />
                                                            <asp:Button ID="btnotherupdate" runat="server" Text="Update" Visible="false"
                                                                Font-Size="Medium" OnClick="btnotherupdate_Click" />
                                                            <asp:Button ID="btnotherdelete" runat="server" Text="Delete" Visible="false" Font-Size="Medium"
                                                                OnClick="btnotherdelete_Click" />
                                                            <asp:Button ID="bthotherexit" runat="server" Text="Exit" Font-Size="Medium" OnClick="bthotherexit_Click" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </center>
                                    </asp:Panel>
                                </center>
                            </div>


                        </asp:Panel>

                        <asp:CollapsiblePanelExtender ID="Collapseother" runat="server" TargetControlID="panelheaderother"
                            CollapseControlID="pnlother" ExpandControlID="pnlother" Collapsed="true"
                            TextLabelID="lblother" CollapsedSize="0" ImageControlID="imageother"
                            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>

                        <center>
                            <asp:Label ID="lablevalidation" runat="server" Visible="false" ForeColor="Red" Font-Names="Book Antiqua"
                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_save" Text="" runat="server" OnClick="btn_save_Click" Style="font-weight: bold; margin-left: 40px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_delete" Text="Delete" runat="server" OnClick="btn_delete_Click" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_appexit" Text="Close" runat="server" OnClick="btn_appexit_Click"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </center>
                    </div>
                </div>
            </fieldset>
        </center>
        <center>
            <div id="Plus1" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                <center>
                    <div id="Div19" runat="server" class="table" style="background-color: White; height: 150px; width: 355px; border: 5px solid #3399ff; border-top: 25px solid #3399ff; margin-top: 200px; border-radius: 10px;">
                        <center>
                            <br />
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_header1" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_add2" runat="server" MaxLength="150" CssClass="textbox txtheight2 txtcapitalize"
                                            Style="font-weight: bold; width: 325px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_plusAdd1" Text=" Add " runat="server" OnClick="btn_plusAdd1_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_ftypeadd1" Text=" Add " Visible="false" runat="server" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_bnameadd1" Text=" Add " Visible="false" runat="server" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_branchadd1" Text=" Add " Visible="false" runat="server" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_Plusexit1" Text=" Exit " runat="server" OnClick="btn_Plusexit1_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <fieldset id="divcall" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%; margin-top: -20px;">
                <asp:ImageButton ID="ImageButton13" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 453px;"
                    OnClick="ImageButton13_Click" />
                <div class="subdivstyle" style="background-color: White; margin-left: -26px; overflow: auto; margin-top: 13px; width: 980px; height: 600px;"
                    align="center">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: indigo;">InterView Call</span>
                        </div>
                    </center>
                    <fieldset id="callfilter" runat="server" style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: 40px; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: 0px; width: 850px;">
                        <table style="margin-left: -70px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_cdept" runat="server" Text="Department : " Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel52" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_cdept" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                <asp:CheckBox ID="cb_cdept" runat="server" Text="Select All" OnCheckedChanged="cb_cdept_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_cdept" runat="server" OnSelectedIndexChanged="cbl_cdept_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_cdept"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_applied" runat="server" Text="Post Applied For : " Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel53" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_cdesig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                <asp:CheckBox ID="cb_cdesig" runat="server" Text="Select All" OnCheckedChanged="cb_cdesig_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_cdesig" runat="server" OnSelectedIndexChanged="cbl_cdesig_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_cdesig"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_type" runat="server" Text="Staff Type : " Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel55" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_cstype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; margin-left: 0px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel8" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                <asp:CheckBox ID="cb_cstype" runat="server" Text="Select All" OnCheckedChanged="cb_cstype_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_cstype" runat="server" OnSelectedIndexChanged="cbl_cstype_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_cstype"
                                                PopupControlID="Panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" OnClick="btn_go_Click" Style="font-weight: bold; margin-left: 10px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="lbl_cerr" runat="server" Visible="false" Style="font-weight: bold; font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                    </center>
                    <div>
                        <center>
                            <farpoint:fpspread id="Fpspread18" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="925px" height="350px" class="spreadborder"
                                visible="false" onbuttoncommand="Fpspread18_Command" showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                        </center>
                    </div>
                    <br />
                    <asp:CheckBox ID="cb_interviewdet" Visible="false" runat="server" Style="margin-left: -786px;"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cb_interviewdet_OnCheckedChanged"
                        AutoPostBack="true" Text="Interview Details" />
                    <center>
                        <asp:Label ID="lbl_alertc" runat="server" Text="" Visible="false" Font-Bold="true"
                            Font-Names="Book Antiqua" Style="color: Red;" Font-Size="Medium"></asp:Label></center>
                    <br />
                    <fieldset id="interview" runat="server" visible="false">
                        <legend>Interview Details </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_date" runat="server" Text="Date" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender20" TargetControlID="txt_date" runat="server"
                                        Format="dd/MM/yyyy" PopupPosition="TopLeft">
                                    </asp:CalendarExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_time" runat="server" Text="Time" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <fieldset>
                                        <asp:Label ID="lbl_hh" runat="server" Text="HH" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddl_hour" runat="server" Width="60px" Enabled="false" CssClass="textbox txtheight1"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_hour_change" Style="height: 30px;"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Position="Bottom">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lbl_mm" runat="server" Text="MM" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddl_mm" runat="server" Enabled="false" Width="60px" CssClass="textbox txtheight1"
                                            Style="height: 30px;" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Position="Bottom">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26</asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28</asp:ListItem>
                                            <asp:ListItem Value="29">29</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="31">31</asp:ListItem>
                                            <asp:ListItem Value="32">32</asp:ListItem>
                                            <asp:ListItem Value="33">33</asp:ListItem>
                                            <asp:ListItem Value="34">34</asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="36">36</asp:ListItem>
                                            <asp:ListItem Value="37">37</asp:ListItem>
                                            <asp:ListItem Value="38">38</asp:ListItem>
                                            <asp:ListItem Value="39">39</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="41">41</asp:ListItem>
                                            <asp:ListItem Value="42">42</asp:ListItem>
                                            <asp:ListItem Value="43">43</asp:ListItem>
                                            <asp:ListItem Value="44">44</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="46">46</asp:ListItem>
                                            <asp:ListItem Value="47">47</asp:ListItem>
                                            <asp:ListItem Value="48">48</asp:ListItem>
                                            <asp:ListItem Value="49">49</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="51">51</asp:ListItem>
                                            <asp:ListItem Value="52">52</asp:ListItem>
                                            <asp:ListItem Value="53">53</asp:ListItem>
                                            <asp:ListItem Value="54">54</asp:ListItem>
                                            <asp:ListItem Value="55">55</asp:ListItem>
                                            <asp:ListItem Value="56">56</asp:ListItem>
                                            <asp:ListItem Value="57">57</asp:ListItem>
                                            <asp:ListItem Value="58">58</asp:ListItem>
                                            <asp:ListItem Value="59">59</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_ampm" runat="server" Enabled="false" CssClass="textbox txtheight1"
                                            Style="height: 30px;" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Width="60px" Position="Bottom">
                                            <asp:ListItem Value="0">AM</asp:ListItem>
                                            <asp:ListItem Value="1">PM</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Button ID="btn_call_letter" Text="Call Letter" runat="server" Visible="false" OnClick="btn_call_letter_Click" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_venue" runat="server" Text="Venue" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_venue" runat="server" Enabled="false" TextMode="multiline" CssClass="textbox txtheight5"
                                        onkeypress="onvenchange()" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_loc" runat="server" Text="Location" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_loc" runat="server" Enabled="false" TextMode="multiline" CssClass="textbox txtheight5"
                                        onkeypress="onvenchange()" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Button ID="btn_call" Text="Interview Call" runat="server" OnClick="btn_call_Click"
                                        Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    <asp:Button ID="btn_cupdate" Text="Interview Call Update" Visible="false" runat="server"
                                        OnClick="btn_cupdate_Click" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <center>
                        <div id="updatecall" runat="server" visible="false">
                            <asp:Button ID="btn_remove" Text="Remove" runat="server" OnClick="btn_remove_Click"
                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_cexit" Text="Exit" runat="server" OnClick="ImageButton13_Click"
                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </div>
                    </center>
                </div>
            </fieldset>
        </center>
        <center>
            <fieldset id="divSelect" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%; margin-top: -20px;">
                <asp:ImageButton ID="ImageButton14" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 453px;"
                    OnClick="ImageButton14_Click" />
                <div class="subdivstyle" style="background-color: White; margin-left: -26px; overflow: auto; margin-top: 13px; width: 980px; height: 600px;"
                    align="center">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: indigo;">Staff Selection</span>
                        </div>
                    </center>
                    <center>
                        <fieldset id="filterstf" runat="server" visible="false" style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px; box-shadow: 0 0 8px #999999; height: 35px; margin-left: 0px; margin-top: 8px; padding: 1em; margin-left: 0px; width: 930px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_scollege" runat="server" Text="College Name : " Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlscollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="310px" AutoPostBack="True" OnSelectedIndexChanged="ddlscollege_Change">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkseldate" runat="server" AutoPostBack="true" OnCheckedChanged="chkseldate_change" />
                                        <asp:Label ID="lbl_sfdate" runat="server" Text="From Date" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stfdate" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                            AutoPostBack="true" OnTextChanged="txt_stfdate_Change" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender25" TargetControlID="txt_stfdate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stdate" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stdate" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                            AutoPostBack="true" OnTextChanged="txt_stdate_Change" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender21" TargetControlID="txt_stdate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_sgo" Text="Go" runat="server" OnClick="btn_sgo_Click" Style="font-weight: bold; margin-left: 10px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <br />
                        <center>
                            <farpoint:fpspread id="Fpspread19" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="925px" height="450px" class="spreadborder"
                                visible="false" onbuttoncommand="Fpspread19_Command" showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                        </center>
                        <br />
                        <asp:CheckBox ID="cb_sinterviewdet" Visible="false" runat="server" Style="margin-left: -786px;"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cb_sinterviewdet_OnCheckedChanged"
                            AutoPostBack="true" Text="Interview Details" />
                        <center>
                            <asp:Label ID="lbl_salert" runat="server" Text="" Visible="false" Font-Bold="true"
                                Font-Names="Book Antiqua" Style="color: Red;" Font-Size="Large"></asp:Label></center>
                        <br />
                        <fieldset id="staffinterview" runat="server" visible="false">
                            <legend style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;">Feedback Details </legend>
                            <table style="margin-left: -155px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_perform" runat="server" Text="Performance" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_stplus" Text="+" OnClick="btn_stplus_OnClick" runat="server"
                                            Width="40px" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel57" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_perform" Enabled="false" AutoPostBack="true" runat="server"
                                                    CssClass="textbox txtheight5" Style="height: 30px;" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="200px">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_stminus" Text="-" OnClick="btn_stminus_OnClick" runat="server"
                                            Width="40px" Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_selectstat" runat="server" Text="Selection Status" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel58" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_selectstatus" Enabled="false" runat="server" Font-Bold="true"
                                                    CssClass="textbox txtheight5" Style="height: 30px;" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="200px" AutoPostBack="True">
                                                    <asp:ListItem>Selected </asp:ListItem>
                                                    <asp:ListItem>Rejected </asp:ListItem>
                                                    <asp:ListItem>Waiting </asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: -477px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_remarks" runat="server" Text="Remarks" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_remarkrs" runat="server" Enabled="false" TextMode="multiline"
                                            Style="margin-left: 26px; width: 280px;" CssClass="textbox txtheight5" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <fieldset style="width: 190px; height: 20px; background-color: #ffccff; margin-left: -650px; border-radius: 10px; border-color: #6699ee;">
                                <asp:RadioButton ID="rdb_int" runat="server" Text="Internal" AutoPostBack="true"
                                    GroupName="a" Checked="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:RadioButton ID="rdb_external" runat="server" Text="External" AutoPostBack="true"
                                    GroupName="a" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </fieldset>
                            <asp:Button ID="btn_interviewer" Text="Select Interviewer" OnClick="btn_interviewer_OnClick"
                                runat="server" Style="font-weight: bold; margin-left: 600px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_staffsave" Text="Save" runat="server" Visible="false" OnClick="btn_staffsave_Click"
                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_selupdate" Text="Update" Visible="false" runat="server" OnClick="btn_selupdate_Click"
                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_selclose" Text="Close" runat="server" OnClick="btn_selclose_Click"
                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <center>
                                <farpoint:fpspread id="Fpspread20" runat="server" overflow="true" bordercolor="Black"
                                    borderstyle="Solid" borderwidth="1px" width="900px" height="200px" class="spreadborder"
                                    visible="false" showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </farpoint:fpspread>
                            </center>
                        </fieldset>
                        <center>
                            <div id="External" runat="server" visible="false" class="popupstyle popupheight1"
                                style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                <center>
                                    <div id="ext1" runat="server" class="table" style="background-color: White; height: 350px; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px; border-radius: 10px;">
                                        <center>
                                            <br />
                                            <center>
                                                <fieldset style="width: 190px; height: 20px; margin-left: 0px; border-radius: 10px; border-color: #6699ee; border: 2px solid indigo;">
                                                    <asp:Label ID="Label7" runat="server" Text="External Details" Style="color: #990099;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </fieldset>
                                            </center>
                                            <br />
                                            <table style="line-height: 30px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_scode" runat="server" Text="Staff Code" Style="width: 200px;"
                                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_stfscode" runat="server" MaxLength="15" CssClass="textbox txtheight2"
                                                            Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender104" runat="server" TargetControlID="txt_stfscode"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','.','/'" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_stfname" runat="server" Text="Name" Style="width: 200px;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_stfname" runat="server" MaxLength="20" CssClass="textbox txtheight2"
                                                            Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender105" runat="server" TargetControlID="txt_stfname"
                                                            FilterType="Custom,uppercaseletters,lowercaseletters" ValidChars="letters  ." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_stfdpt" runat="server" Text="Address" Style="width: 200px;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdpt" runat="server" MaxLength="20" CssClass="textbox txtheight2"
                                                            Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender106" runat="server" TargetControlID="txtdpt"
                                                            FilterType="Custom,uppercaseletters,lowercaseletters" ValidChars="letters  ,&." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_stfaddress" runat="server" Text="Email" Style="width: 200px;"
                                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_address" runat="server" MaxLength="30" CssClass="textbox txtheight2"
                                                            onblur="return checkEmail(this)" Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_stfphone" runat="server" Text="Phone No" Style="width: 200px;"
                                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_phone" runat="server" MaxLength="11" CssClass="textbox txtheight2"
                                                            Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium; margin-left: 20px;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender107" runat="server" TargetControlID="txt_phone"
                                                            FilterType="Custom" ValidChars="0123456789 +" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <div>
                                                <center>
                                                    <asp:Button ID="btn_extadd" Text=" Add " OnClick="btn_extadd_OnClick" runat="server"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                    <asp:Button ID="btn_extexit" Text=" Exit " runat="server" OnClick="btn_extexit_OnClick"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                </center>
                                            </div>
                                        </center>
                                    </div>
                                </center>
                            </div>
                        </center>
                        <center>
                            <div id="StfInternal" runat="server" visible="false" class="popupstyle popupheight1"
                                style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                                <center>
                                    <div id="stfdiv" runat="server" class="table" style="background-color: White; height: 530px; width: 800px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 50px; border-radius: 10px;">
                                        <center>
                                            <br />
                                            <center>
                                                <fieldset style="width: 750px; height: 100px; margin-left: 0px; border-radius: 10px; border-color: #6699ee; border: 2px solid indigo;">
                                                    <fieldset style="width: 190px; height: 20px; margin-left: 0px; border-radius: 10px; border-color: #6699ee; border: 2px solid indigo;">
                                                        <asp:Label ID="Label8" runat="server" Text="Internal Details" Style="color: #990099;"
                                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </fieldset>
                                                    <br />
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_stfstream" runat="server" Text="Stream" Font-Bold="true" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel59" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="ddl_stfstream" runat="server" Font-Bold="true" CssClass="textbox txtheight5"
                                                                            Style="height: 30px;" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px"
                                                                            AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_stfsearchby" runat="server" Text="Search By" Font-Bold="true"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel60" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="ddl_stfsearchby" runat="server" Font-Bold="true" OnSelectedIndexChanged="ddl_stfsearchby_OnSelectedIndexChanged"
                                                                            CssClass="textbox txtheight5" Style="height: 30px;" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="150px" AutoPostBack="True">
                                                                            <asp:ListItem>All</asp:ListItem>
                                                                            <asp:ListItem>Staff Code</asp:ListItem>
                                                                            <asp:ListItem>Staff Name</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="ddl_stfsearchby" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_stfsearchbysc" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                    Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stfsearchbysc"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="txtsearchpan">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_stfsearchbysn" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                    Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium; margin-left: 0px;"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stfsearchbysn"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="txtsearchpan">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_stfintgo" Text=" Go " runat="server" OnClick="btn_stfintgo_OnClick"
                                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </center>
                                            <center>
                                                <asp:Label ID="int_stfalert" runat="server" Text="" Visible="false" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Style="color: Red;" Font-Size="Medium"></asp:Label></center>
                                            <br />
                                            <center>
                                                <farpoint:fpspread id="Fpspread21" runat="server" overflow="true" bordercolor="Black"
                                                    borderstyle="Solid" borderwidth="1px" width="750px" height="300px" class="spreadborder"
                                                    showheaderselection="false" style="border-radius: 10px; margin-left: 1px;">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </farpoint:fpspread>
                                            </center>
                                            <br />
                                            <div>
                                                <center>
                                                    <asp:Button ID="btn_stfintAdd" Text=" Add " OnClick="btn_stfintAdd_OnClick" runat="server"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                    <asp:Button ID="btn_stfintexit" Text=" Exit " runat="server" OnClick="btn_stfintexit_OnClick"
                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                                </center>
                                            </div>
                                        </center>
                                    </div>
                                </center>
                            </div>
                        </center>
                    </center>
                </div>
            </fieldset>
        </center>
        <center>
            <div id="Plus" runat="server" visible="false" class="popupstyle popupheight1" style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 150px; width: 238px; border: 5px solid #3399ff; border-top: 25px solid #3399ff; margin-top: 200px; border-radius: 10px;">
                        <center>
                            <br />
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_header" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_add" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_add" Text=" Add " runat="server" OnClick="btn_add_OnClick" Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_exit" Text=" Exit " runat="server" OnClick="btn_exit_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="AnsPlus" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 150px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                        <center>
                            <br />
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_ansheader" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_answer" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addans" Text=" Add " runat="server" OnClick="btn_addans_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                        <asp:Button ID="btn_ansexit" Text=" Exit " runat="server" OnClick="btn_ansexit_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: -167px; left: 0px;">
                <center>
                    <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" Visible="false" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                Text="Yes" runat="server" OnClick="btn_yes_Click" />
                                            <asp:Button ID="btn_no" Visible="false" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                Text="No" runat="server" OnClick="btn_no_Click" />
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
            <div id="photo_div" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="Div18" runat="server" class="table" style="background-color: White; height: 300px; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 110px; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label9" runat="server" Text="Add Staff Photo" Style="color: Red;"
                                Font-Bold="true" Font-Size="large"></asp:Label>
                        </center>
                        <center>
                            <br />
                            <br />
                            <table style="margin-left: 75px;">
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="fileuploadbrowse" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_photoupload" Text="UpLoad" runat="server" OnClick="btn_photoupload_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btn_uploadclose" Text="Close" runat="server" OnClick="btn_uploadclose_OnClick"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px; margin-left: -150px;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="signdiv" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="Div20" runat="server" class="table" style="background-color: White; height: 300px; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 110px; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label10" runat="server" Text="Add Signature" Style="color: Red;" Font-Bold="true"
                                Font-Size="large"></asp:Label>
                        </center>
                        <center>
                            <br />
                            <br />
                            <table style="margin-left: 75px;">
                                <tr>
                                    <td>
                                        <asp:Image ID="img_sign" runat="server" Width="100px" Height="128px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:FileUpload ID="fileuploadsign" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Button ID="btn_signupload" Text="UpLoad" runat="server" OnClick="btn_signupload_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="btn_signclose" Text="Close" runat="server" OnClick="btn_signclose_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </center>
                    </div>
                </center>
            </div>
        </center>

        <%--alert div for priority--%>
        <center>
            <div id="alertpopuppriority" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="Div21" runat="server" class="table" style="background-color: White; height: auto; width: 264px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 220px; border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alertpriority" runat="server" align="center" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_priorityyes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="Yes" runat="server" OnClick="btnPriorityYes_Click" />


                                            <asp:Button ID="btn_priorityno" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="No" runat="server" OnClick="btnPriorityNo_Click" />

                                            <asp:Button ID="btn_priorityUpdateYes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="Yes" runat="server" OnClick="btnPriorityUpdateYes_Click" />


                                            <asp:Button ID="btn_priorityUpdateNo" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="No" runat="server" OnClick="btnPriorityUpdateNo_Click" />



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
            <div id="divShowStudDet" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: absolute; width: 100%; z-index: 3000; height: 100em;">
                <asp:ImageButton ID="ImageButton16" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 395px;"
                    OnClick="ImageButton16_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; margin-left: 0px; overflow: auto; width: 830px; height: auto;"
                    align="center">
                    <br />
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: auto; border-radius: 10px; border: 1px solid Gray;">
                        <center>
                            <span class="fontstyleheader" style="color: indigo;">Staff Children Details</span>
                        </center>
                        <br />
                        <center>
                            <farpoint:fpspread id="Fpspread23" runat="server" overflow="true" bordercolor="Black"
                                borderstyle="Solid" borderwidth="1px" width="820px" height="175px" class="spreadborder"
                                visible="false" showheaderselection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </farpoint:fpspread>
                        </center>
                        <br />
                        <center>
                            <div id="divStudRep" runat="server" visible="false" style="font-weight: bold;">
                                <asp:Label ID="lblValStudRep" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label><br />
                                <asp:Label ID="lblStudRep" runat="server" Text="Report Name" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                                <asp:TextBox ID="txtStudxl" runat="server" Width="180px" onkeypress="display()"
                                    CssClass="textbox txtheight2 txtcapitalize" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:Button ID="btnStudXL" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                                    OnClick="btnStudXL_Click" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender110" runat="server" TargetControlID="txtStudxl"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnStudPrint" runat="server" Text="Print" CssClass="textbox textbox1"
                                    OnClick="btnStudPrint_Click" Width="60px" Height="30px" Style="font-weight: bold; font-family: Book Antiqua; font-size: medium;" />
                                <Insproplus:printmaster runat="server" ID="Printmaster10" Visible="false" />
                            </div>
                        </center>
                    </div>
                    <br />
                    <br />
                </div>
            </div>
        </center>

        <center>
            <div id="LicenseDiv" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="Div22" runat="server" class="table" style="background-color: White; height: 300px; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 110px; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label11" runat="server" Text="Add License" Style="color: Red;" Font-Bold="true"
                                Font-Size="large"></asp:Label>
                        </center>
                        <center>
                            <br />
                            <table style="margin-left: 50px;">
                                <tr>
                                    <td colspan="3">
                                        <asp:Image ID="ImgLicFront" runat="server" Width="100px" Height="128px" />
                                    </td>

                                    <td colspan="3">

                                        <asp:Image ID="ImgLicBack" runat="server" Width="100px" Height="128px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblAddLicFront" runat="server" Text="Add License Front" Style="color: Black;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:FileUpload ID="fileuploadLicFront" runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblAddLicBack" runat="server" Text="Add License Back" Style="color: Black;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>

                                        <asp:FileUpload ID="fileuploadLicBack" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="LblRenewDt" runat="server" Text="License Renew Date" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLicRenewDt" runat="server" AutoPostBack="true"
                                            OnFocus="return myFunction(this)" CssClass="textbox txtheight2" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                        </asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender23" TargetControlID="txtLicRenewDt" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                            <asp:Button ID="LicenseUpload" Text="UpLoad" runat="server" OnClick="btn_LicenseUpload_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="LicenseSave" Text="Save" runat="server" OnClick="LicenseSave_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            <asp:Button ID="LicenseClose" Text="Close" runat="server" OnClick="btn_LicenseClose_OnClick"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" class="popupstyle popupheight1"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto; width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 220px; border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
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
    </body>
    </html>
</asp:Content>
