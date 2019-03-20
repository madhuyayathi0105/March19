<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true" CodeFile="StudentApplicationReportliba.aspx.cs" Inherits="StudentApplicationReportliba" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 1000px;
            width: 1000px;
        }
    </style>
    <style type="text/css">
        .ajax__myTab
        {
            text-align: center;
        }
        .ajax__myTab .ajax__tab_header
        {
            font-family: Book Antiqua;
            text-align: initial;
            font-size: 16px;
            font-weight: bold;
            color: White;
            border-left: solid 1px #666666;
            border-bottom: thin 1px #666666;
        }
        .ajax__myTab .ajax__tab_outer
        {
            border: 1px solid black;
            width: 220px;
            height: 35px;
            border-top: 3px solid transparent;
        }
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 4px;
            background-color: indigo;
            width: 275px;
            height: 35px;
        }
        
        .ajax__myTab .ajax__tab_tab
        {
            height: 22px;
            padding: 4px;
            margin: 0;
            text-align: center;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            border-top: 3px solid #00527D;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #A1C344;
            color: White;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
            background-color: #A1C344;
            cursor: pointer;
            color: White;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            border-top: 2px solid white;
            border-bottom: transparent;
            color: #B0E0E6;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #F36200;
            border-bottom: transparent;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
            background-color: #F36200;
            cursor: inherit;
            width: 160px;
        }
        .ajax__myTab .ajax__tab_body
        {
            border: 1.5px solid #F36200;
            padding: 6px;
            background-color: #EFEBEF;
        }
        .ajax__myTab .ajax__tab_disabled
        {
            color: #F1F1F1;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <script type="text/javascript">
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
            var fvalue = "";
            var tovalue = "";
            fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;
            fvalue = document.getElementById('<%=txt_fromdate.ClientID %>');
            tovalue = document.getElementById('<%=txt_todate.ClientID %>');
            var today = new Date();
            var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);

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
                fvalue.value = currentDate;
                tovalue.value = currentDate;
                alert("To date should be greater than from date ");
                return false;
            }
        
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green; font-size: x-large;">Student Application
                        Manager</span>
              
                </div>
            </center>
        </div>
    </center>
    <center>
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                AutoPostBack="true" Width="140px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlgrad" CssClass="ddlheight4 textbox textbox1" runat="server"
                                AutoPostBack="true" Width="140px" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" Width="100px" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="ddlheight textbox textbox1"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p3" runat="server" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_degree_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="p3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="p4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_searchstudname" runat="server" Text="Student Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_searchstudname" runat="server" CssClass="textbox textbox1 txtheight2"
                                Width="128px" AutoPostBack="true" OnTextChanged="txt_searchstudname_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchstudname" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_searchstudname"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_searchappno" runat="server" Text="Application No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight1"
                                Width="110px" AutoPostBack="true" OnTextChanged="txt_searchappno_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                                FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_searchmobno" runat="server" Text="Mobile No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_searchmobno" runat="server" CssClass="textbox textbox1 txtheight2"
                                Width="135px" MaxLength="13" AutoPostBack="true" OnTextChanged="txt_searchmobno_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender01" runat="server" TargetControlID="txt_searchmobno"
                                FilterType="numbers,custom" ValidChars=" +">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getmob" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchmobno"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="12">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbl_datewise" AutoPostBack="true" OnCheckedChanged="cb_selectedchange_Click"
                                                        runat="server" Text="Date" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" onchange="return checkDate()"
                                                        Style="height: 20px; width: 67px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                                    <asp:TextBox ID="txt_todate" runat="server" Enabled="false" onchange="return checkDate()"
                                                        Style="height: 20px; width: 67px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" Text="Report Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreportTye" CssClass="ddlheight4 textbox textbox1" runat="server"
                                            AutoPostBack="true" Width="145px" OnSelectedIndexChanged="ddlreportTye_SelectedIndexChanged">
                                            <asp:ListItem>Applied</asp:ListItem>
                                            <asp:ListItem>Shortlist</asp:ListItem>
                                            <asp:ListItem>Wait to Admitted</asp:ListItem>
                                            <asp:ListItem>Admitted</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go" runat="server" Text="Search" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_go_OnClick" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdb_paid" runat="server" Text="Paid" Checked="true" GroupName="d" />
                                        <asp:RadioButton ID="rdb_notpaid" runat="server" Text="Not Paid" GroupName="d" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <br />
        <br />
        <div id="grid" runat="server">
            <%--style="width:1500px;"--%>
            <asp:GridView ID="gridDetail" runat="server" Visible="false" OnDataBound="gridDetail_OnDataBound"
                OnRowDataBound="gridDetail_OnRowDataBound">
            </asp:GridView>
            <br />
            <asp:Button ID="btnExport" runat="server" Visible="false" Text="Export to Excel"
                OnClick="btnExport_Click" />
        </div>
    </center>
    <%-- </div>--%>
    </center>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 100000;
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
                                    <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
</asp:Content>

