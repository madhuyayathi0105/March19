<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HT_HostelRegistration.aspx.cs" Inherits="HT_HostelRegistration" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 500px;
            width: 1000px;
        }
        .lnk:hover
        {
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            color: Green;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function change1(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_reason.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_reason.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function change2(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_susreason.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_susreason.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function valid1() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                id = document.getElementById("<%=txt_pop1name.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1name.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_pop1roomno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1roomno.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function valid() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=txt_room.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_room.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_reason.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_reason.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_reason.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_reason.ClientID %>");
                    idval.style.borderColor = 'Red';
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function validsus() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=ddl_susreason.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_susreason.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_susreason.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_susreason.ClientID %>");
                    idval.style.borderColor = 'Red';
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000;">Hostel Registration</span>
                    </div>
                </center>
                <br />
                <div class="maindivstyle" style="height: auto; width: 1000px;">
                    <center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server"></asp:Label>
                                </td>
                            <td>
                                    <asp:UpdatePanel ID="Upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="College" runat="server" CssClass="txtheight"></asp:Label>
                                </td>
                                <td>
                                    <%--  <asp:DropDownList ID="ddl_college" Visible="false" runat="server" CssClass="textbox  ddlheight4"
                                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>--%>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_college" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 260px; height: 200px;">
                                                <asp:CheckBox ID="cb_clg" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_clg_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_clg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_clg_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_college"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_roll" Text="Roll No" runat="server" CssClass="txtheight"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight3 textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_roll"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_name" Text="Name" runat="server" CssClass="txtheight2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight3 textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_name"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb1" runat="server" OnCheckedChanged="cb1_CheckedChanged" AutoPostBack="true" />
                                    <asp:Label ID="lblbatch" Text="Batch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_batch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="200px" Height="200px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="p4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Mess Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_basemesstype" runat="server" CssClass="ddlheight textbox1"
                                        Width="120px">
                                        <asp:ListItem Value="2">Both</asp:ListItem>
                                        <asp:ListItem Value="0">Veg</asp:ListItem>
                                        <asp:ListItem Value="1">Non veg</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbuildname" Width="102px" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_buildname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_buildname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_buildname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_buildname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_buildingname"
                                                PopupControlID="p5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_floorname" Text="Floor Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_floorname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_floorname"
                                                PopupControlID="p6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_roomname" Text="Room Name" Width="86px" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_roomname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroomname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_roomname"
                                                PopupControlID="p7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_vacate" runat="server" OnCheckedChanged="cb_vacate_CheckedChanged"
                                        AutoPostBack="true" Width="117px" Text="Vacated Date" />
                                    <%--<asp:Label ID="Lbl_vacatedate"  runat="server" Text="Vacated Date"></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_vacate" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_vacate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <div id="vacateformdate_div" runat="server" visible="false">
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_vacatedatebetween" runat="server" Width="117px" Text="Vacated date"
                                            AutoPostBack="true" OnCheckedChanged="cb_vacatedatebetween_onclick" />
                                    </td>
                                    <td>
                                        <span>From Date</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfrmdate" Enabled="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:CalendarExtender ID="calfrmdate" runat="server" TargetControlID="txtfrmdate"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <span>To Date</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" Enabled="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodate" CssClass="cal_Theme1 ajax__calendar_active"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_sex" Text="Gender" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sex" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                            <asp:Panel ID="p11" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_sex" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sex_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sex" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sex_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Male</asp:ListItem>
                                                    <asp:ListItem Value="1">Female</asp:ListItem>
                                                    <asp:ListItem Value="2">Transgender</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_sex"
                                                PopupControlID="p11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_community" Text="Community" Width="84px" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_community" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="200px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_community" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_community_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_community" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_community"
                                                PopupControlID="p8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_status" Text="Status" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_status" runat="server" CssClass="textbox txtheight1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="P10" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_status" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_status_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_status" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_status_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Suspension </asp:ListItem>
                                                    <asp:ListItem Value="1">Discontinue </asp:ListItem>
                                                    <asp:ListItem Value="2">Vacated</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_status"
                                                PopupControlID="p10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                        OnClick="btn_go_Click" Style="float: right;" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_add" Text="Add New" CssClass="textbox btn2" runat="server" OnClick="btn_add_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <center>
                        <asp:Label ID="lbl_errmsg" runat="server" Style="color: Red;"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="940px" Style="margin-top: -0.1%;">
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
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="940px">
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
                                            <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Roll_Admit">Admission No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Degree">Course</asp:ListItem>
                                            <asp:ListItem Value="Reg_No">Reg No</asp:ListItem>
                                            <asp:ListItem Value="DOB">DOB</asp:ListItem>
                                            <asp:ListItem Value="Parent_AddressP">Address</asp:ListItem>
                                            <asp:ListItem Value="PAddress">Street</asp:ListItem>
                                            <asp:ListItem Value="CityP">City</asp:ListItem>
                                            <asp:ListItem Value="Ppincode">Pincode</asp:ListItem>
                                            <asp:ListItem Value="districtp">District</asp:ListItem>
                                            <asp:ListItem Value="StateP">State</asp:ListItem>
                                            <asp:ListItem Value="community">Community</asp:ListItem>
                                            <asp:ListItem Value="region">Religion</asp:ListItem>
                                            <asp:ListItem Value="Admin_Date">Admit Date</asp:ListItem>
                                            <asp:ListItem Value="BuildingFK">Building</asp:ListItem>
                                            <asp:ListItem Value="FloorFK">Floor</asp:ListItem>
                                            <asp:ListItem Value="RoomFK">Room</asp:ListItem>
                                            <asp:ListItem Value="Room_type">Room Type</asp:ListItem>
                                            <asp:ListItem Value="DiscontinueDate">Discontinue</asp:ListItem>
                                            <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Student_Mobile">Student Mobile</asp:ListItem>
                                            <asp:ListItem Value="StudMessType">Student Mess Type</asp:ListItem>
                                             <asp:ListItem Value="id">Student Id</asp:ListItem>
                                            <%--<asp:ListItem Value="ParentM_Mobile">Mother Mobile</asp:ListItem>--%>
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
                    <br />
                    <center>
                        <asp:Label Style="color: Red;" ID="lblerr" Text="Record Not Found" runat="server"></asp:Label>
                    </center>
                    <p style="width: 840px; margin-top: 0px;" align="right">
                        <asp:Label ID="lbl_stucnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <center>
                        <div id="div1" runat="server" visible="false" style="width: 923px; height: 350px;
                            background-color: White;" class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" OnCellClick="FpSpread1_CellClick"
                                OnPreRender="FpSpread1_SelectedIndexChanged" OnUpdateCommand="Fpspread1_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                    </center>
                    <p style="width: 691px;" align="left">
                        <asp:Label ID="lbl_vatreason" Visible="false" runat="server" Text="Vacated Reason"></asp:Label>
                        <asp:TextBox ID="txt_vatreason" placeholder="Vacated Reason" Visible="false" runat="server"
                            CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                        <asp:Button ID="btn_vacate" Text="Vacate" CssClass="textbox btn2" runat="server"
                            OnClick="btn_vacate_Click" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_vacated" Visible="false" runat="server" Text="Withdrawal Form"
                            CssClass="lnk" OnClick="lnk_vacated_Click"> </asp:LinkButton>
                    </p>
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <div id="div_report" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                                AutoPostBack="true" OnClick="btnExcel_Click" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                                AutoPostBack="true" OnClick="btn_printmaster_Click" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </div>
                    <br />
                </div>
            </center>
            <center>
                <div id="popwindow1" runat="server" class="popupstyle" visible="false" style="height: 61em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 443px;"
                        OnClick="imagebtnpop1close_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 700px; width: 915px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader ">Hostel Registration</span></div>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1hostelname" Text="Hostel Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop1hostelname" runat="server" CssClass="textbox ddlheight4 textbox1" AutoPostBack="true"
                                            Width="183px" onfocus="return myFunction(this)" OnSelectedIndexChanged="ddl_pop1hostelname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                      <td>
                                        <asp:Label ID="Lblmess" Text="Mess Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlmess" runat="server" CssClass="textbox ddlheight4 textbox1"
                                            Width="152px" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1rollno" Text="Roll No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1rollno" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px" AutoPostBack="true"
                                            OnTextChanged="roll_txtchange" MaxLength="30"></asp:TextBox>
                                        <asp:Button ID="btn1" Text="?" runat="server" OnClick="btn1_Click" CssClass="textbox btn" />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>

                                     <td>
                                        <asp:Label ID="lblid" Text="Student Id" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px"  Enabled="false"
                                             MaxLength="30"></asp:TextBox>
                                      
                                        
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1rolladmin" Text="Roll Admit" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1rolladmin" runat="server" Visible="false" ReadOnly="true"
                                            BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1collegename" Text="College Name" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddl_pop1collegename" runat="server" CssClass="textbox ddlheight6 textbox1"
                                            Width="263px" Enabled="false">
                                            <%--OnSelectedIndexChanged="ddl_pop1collegename_selected_indexchange" onfocus="return myFunction(this)"--%>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1name" Text="Name" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_pop1name" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true"></asp:TextBox><%--AutoPostBack="true" OnTextChanged="stud_name_txtchanged"--%>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_pop1name"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                        <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1name"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>--%>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1address" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_pop1address" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1address1" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1address2" runat="server" CssClass="textbox txtheight3 textbox1"
                                            BackColor="#DCF9D1" ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1pin" runat="server" ReadOnly="true" BackColor="#DCF9D1"
                                            CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)" MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_pop1pin"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1community" Text="Community" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1community" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1degree" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1degree" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1dob" Text="DOB" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_date"
                                            FilterType="Numbers,custom" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:DropDownList ID="ddl_pop1date" runat="server" Visible="false" CssClass="textbox textbox1"
                                            BackColor="#DCF9D1" AutoPostBack="true" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_pop1month" runat="server" Visible="false" CssClass="textbox ddlheight textbox1"
                                            BackColor="#DCF9D1" AutoPostBack="true" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_pop1year" runat="server" Visible="false" CssClass="textbox ddlheight textbox1"
                                            BackColor="#DCF9D1" AutoPostBack="true" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1mob" Text="Mobile No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1mob" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)" MaxLength="10"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_pop1mob"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1blood" Text="Blood Group" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1blood" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1admindate" Text="Admit Date" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1admindate" runat="server" Width="80px" BackColor="#DCF9D1"
                                            CssClass="textbox txtheight3"></asp:TextBox>
                                        <asp:CalendarExtender ID="caladmin" TargetControlID="txt_pop1admindate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                        <%-- <asp:DropDownList ID="ddlpop1ddadm" runat="server" Width="40px" CssClass="textbox ddlstyle" AutoPostBack="true" onfocus="return myFunction(this)"></asp:DropDownList>
                            <asp:DropDownList ID="ddlpop1dmadm" runat="server" Width="40px" CssClass="textbox ddlstyle" AutoPostBack="true" onfocus="return myFunction(this)"></asp:DropDownList>
                            <asp:DropDownList ID="ddlpop1yradm" runat="server" Width="80px" CssClass="textbox ddlstyle"  AutoPostBack="true" onfocus="return myFunction(this)"></asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1roomno" Text="Room No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1roomno" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:Button ID="btn2" Text="?" runat="server" OnClick="btn2_Click" CssClass="textbox btn" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1studenttype" Text="Student Type" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2px">
                                        <%--<asp:RadioButton ID="rdbveg" runat="server" Text="Veg" GroupName="same2" />
                                <asp:RadioButton ID="rdbnonveg" runat="server" Text="Non Veg" GroupName="same2" />
                                        <asp:RadioButtonList ID="Radiobtnstype" runat="server" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Value="0">Veg</asp:ListItem>
                                            <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1building" Text="Building Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1building" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_pop1building"
                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1floor" Text="Floor" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1floor" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1roomtype" Text="Room Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1roomtype" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight2 textbox1"></asp:TextBox><%--onfocus="return myFunction(this)"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1discontinue" Text="Discontinue" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_discontinue" runat="server" OnCheckedChanged="cb_discontinue_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:Label ID="lbl_pop1date" Text="Date" runat="server"></asp:Label>
                                        <asp:TextBox ID="txt_discontinuedate" runat="server" CssClass="textbox textbox1"
                                            Width="80px"></asp:TextBox>
                                        <asp:CalendarExtender ID="caldisdate" TargetControlID="txt_discontinuedate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pop1reason" Text="Reason" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1reason" runat="server" CssClass="textbox txtheight3 textbox1"
                                            onfocus="return myFunction(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1vacate" Text="Vacated" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_pop1vacate" runat="server" OnCheckedChanged="cb_pop1vacate_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:Label ID="lbl_pop1date1" Text="Date" runat="server"></asp:Label>
                                        <asp:TextBox ID="txt_vacatedate" runat="server" CssClass="textbox txtheight2" Width="80px"></asp:TextBox>
                                        <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_vacatedate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkbtn_transferhos" runat="server" CssClass="lnk" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Blue" CausesValidation="False"
                                            OnClick="lnkbtn_transferhos_Click" Visible="false">Transfer Hostel</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkbtn_suspension" runat="server" CssClass="lnk" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Blue" CausesValidation="False"
                                            OnClick="lnkbtn_suspension_Click" Visible="false">Suspension Details</asp:LinkButton>
                                    </td>
                                    <td colspan="2">
                                        <asp:LinkButton ID="linkwithdrawal" CssClass="lnk" Visible="false" runat="server"
                                            OnClick="linkwithdrawal_click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Blue" CausesValidation="False">Withdrawal Form</asp:LinkButton>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_Hostelfeesallot" Text="Hostel Fees" runat="server" Visible="false"
                                            AutoPostBack="true" OnCheckedChanged="cb_Hostelfeesallot_Checked" />
                                    </td>
                                    <td runat="server" id="mulsemTD" visible="false">
                                        <asp:UpdatePanel ID="UpdatePanel1mul1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtmulsem" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlmulsem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 150px;">
                                                    <asp:CheckBox ID="cbmulterm" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbmulterm_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblmulterm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblmulterm_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender61" runat="server" TargetControlID="txtmulsem"
                                                    PopupControlID="pnlmulsem" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_pop1save" Text="Save" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1save_Click" OnClientClick="return valid1()" />
                                        <asp:Button ID="btn_pop1exit" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1exit_Click" />
                                        <asp:Button ID="btn_pop1update" Text="Update" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1update_Click" />
                                        <asp:Button ID="btn_pop1delete" Text="Delete" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1delete_Click" />
                                        <asp:Button ID="btn_pop1exit1" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1exit1_Click" />
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <asp:Label ID="errmsg" Style="color: Red;" runat="server"></asp:Label>
                            </center>
                        </center>
                    </div>
            </center>
        </div>
        <center>
            <div id="popwindow2" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                    OnClick="imagebtnpop2close_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 840px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span style="color: Green;" class="fontstyleheader">Select the Student</span></div>
                        <br />
                    </center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2collgname" Text="College Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2collgname_selectedindexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2degre" Text="Degree" runat="server" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2degre" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    OnSelectedIndexChanged="ddl_pop2degre_SelectedIndexChanged" AutoPostBack="true"
                                    onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2batchyr" Text="Batch Year" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2branch" runat="server" CssClass="textbox ddlheight5 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2sex" Text="Sex" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2sex" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2studenttype" Text="Student Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2studenttype" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                    <asp:ListItem Value="Hostler','Day Scholar">Both</asp:ListItem>
                                    <asp:ListItem Value="Hostler">Hostler</asp:ListItem>
                                    <asp:ListItem Value="Day Scholar">Day Scholar</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_pop2go" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_pop2go_Click" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div>
                            <asp:Label ID="lblpop2error" runat="server" ForeColor="Red" Visible="false">
                            </asp:Label>
                        </div>
                    </center>
                    <div style="width: 250px; float: right;">
                        <asp:Label ID="lblcounttxt" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                        <asp:Label ID="lblcount" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                    </div>
                    <br />
                    <br />
                    <center>
                        <%--<div id="div2" runat="server" style="overflow: auto; width: 780px; height: 260px;
                            border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;">--%>
                        <FarPoint:FpSpread ID="fproll" runat="server" Visible="false" Style="overflow: auto;
                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <%-- </div>--%>
                    </center>
                    <br />
                    <asp:Button ID="btn_pop2ok" Text="Ok" runat="server" CssClass="textbox btn2" OnClick="btn_pop2ok_Click" />
                    <asp:Button ID="btn_pop2exit" Text="Exit" runat="server" CssClass="textbox btn2"
                        OnClick="btn_pop2exit_Click" />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow3" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 394px;"
                    OnClick="imagebtnpop3close_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 820px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span style="color: Green;" class="fontstyleheader">Select the Room</span></div>
                        <br />
                    </center>
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pop3buildingname" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3build" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p4444" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                Width="150px" Height="180px" CssClass="multxtpanel" BorderWidth="2px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_pop3build" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3build_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3build" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3build_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender123" runat="server" TargetControlID="txt_pop3build"
                                                PopupControlID="p4444" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%--<td><asp:DropDownList ID="ddlbuildingname" runat="server" CssClass="textbox ddlstyle"  OnSelectedIndexChanged="ddlbuildingname_SelectedIndexChanged"></asp:DropDownList></td>--%>
                                <td>
                                    <asp:Label ID="lbl_pop3floor" Text="Floor" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3floor" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="P1111" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_pop3floor" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3floor_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3floor_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_pop3floor"
                                                PopupControlID="p1111" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_pop3roomtype" Text="Room Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3roomtype" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2222" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 190px;">
                                                <asp:CheckBox ID="cb_pop3roomtype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3roomtype_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3roomtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3roomtype_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_pop3roomtype"
                                                PopupControlID="p2222" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pop3vaccant" Text="Vacant Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_pop3vaccant" runat="server" Width="125px" CssClass="textbox ddlheight2 textbox1">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Filled</asp:ListItem>
                                        <asp:ListItem>Un Filled</asp:ListItem>
                                        <asp:ListItem>Partially Filled</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblinclude" Text="Include:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chck1" runat="server" Text="All" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnCheckedChanged="chck1_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="roomchecklist" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" Font-Names="Book Antiqua" OnSelectedIndexChanged="roomchecklist_SelectedIndexChanged"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                        <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                        <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_gopop3" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_gopop3_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lblpop3err" runat="server" Style="color: Red;"></asp:Label></center>
                    <br />
                    <div id="div3" runat="server" style="width: 810px; height: 180px; overflow: auto">
                        <center>
                            <FarPoint:FpSpread ID="FpSpread3" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" Style="width: 810px; height: 180px;
                                overflow: auto">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread></center>
                    </div>
                    <br />
                    <center>
                        <table class="maintablestyle" runat="server" id="tblStatus" style="border-bottom-style: solid;
                            border-top-style: solid; border-left-style: solid; border-width: 0px;" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="toalrooms" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="totalvaccants" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%-- <asp:Button ID="Button4" runat="server" Width="20px" BackColor="GreenYellow" />--%>
                                    <asp:Label ID="gy" runat="server" Width="20px" Height="20px" BackColor="GreenYellow"></asp:Label>
                                    <asp:Label ID="fill" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="109px"></asp:Label>
                                    <%-- <asp:Button ID="Button5" runat="server" Width="20px" BackColor="Coral" />--%>
                                    <asp:Label ID="cor" runat="server" Width="20px" Height="20px" BackColor="Coral"></asp:Label>
                                    <asp:Label ID="partialfill" runat="server" Text="Partially Filled" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                    <%-- <asp:Button ID="Button6" runat="server" Width="20px" BackColor="MistyRose" />--%>
                                    <asp:Label ID="mis" runat="server" Width="20px" Height="20px" BackColor="MistyRose"></asp:Label>
                                    <asp:Label ID="unfill" runat="server" Text="Unfilled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="145px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_pop3save" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_pop3save_Click" />
                            <asp:Button ID="btn_pop3exit" Text="Exit" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_pop3exit_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
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
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 386px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 796px;
                    height: 350px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_itemcostmaster" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Transfer Hostel Details"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 770px; height: 250px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_trhosdate" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_trhosdate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fromhostel" runat="server" Text="From Hostel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_fromhostel" runat="server" CssClass="textbox  ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_tohostel" runat="server" Text="To Hostel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_tohostel" runat="server" CssClass="textbox  ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_room" Text="Room No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_room" runat="server" onfocus="return myFunction(this)" BackColor="#DCF9D1"
                                            CssClass="textbox txtheight3 textbox1" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="roomnum" runat="server" style="color: Red;">*</span>
                                        <asp:Button ID="btntransferhos" Text="?" runat="server" OnClick="btntransferhos_Click"
                                            CssClass="textbox btn" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_buildingguest" Text="Building Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_building" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_building"
                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_floorguest" Text="Floor" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_floor" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_roomtype" Text="Room Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_roomtype" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight2 textbox1"
                                            ReadOnly="true"></asp:TextBox><%--onfocus="return myFunction(this)"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_reason" runat="server" Text="Reason"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_reason" runat="server" CssClass="textbox  ddlheight3" Style="float: left;"
                                            onchange="change1(this)" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_reason" CssClass="textbox txtheight3 textbox1" Style="display: none;
                                            float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <center>
                            <div>
                                <asp:Button ID="btn_savetrhos" runat="server" CssClass="textbox btn2" Text="Save"
                                    OnClick="btn_savetrhos_Click" OnClientClick="return valid()" />
                                <asp:Button ID="btn_exittrhos" runat="server" CssClass="textbox btn2" Text="Exit"
                                    OnClick="btn_exittrhos_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow3trhos" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 394px;"
                    OnClick="imagebtnpop3closeguest_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 820px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span style="color: #008000; font-weight: bold">Select the Room</span></div>
                        <br />
                    </center>
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1t" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_build" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_build" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_build_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_build" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_build_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_build"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%--<td><asp:DropDownList ID="ddlbuildingname" runat="server" CssClass="textbox ddlstyle"  OnSelectedIndexChanged="ddlbuildingname_SelectedIndexChanged"></asp:DropDownList></td>--%>
                                <td>
                                    <asp:Label ID="Labelt1" Text="Floor" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floorguest" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_floor" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_floor_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floor_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txt_floorguest"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="Labelt2" Text="Room Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelro" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_roomtypeguest" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="panelbackground" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_roomtype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_roomtype_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_roomtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3roomtype_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txt_roomtypeguest"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labelt3" Text="Vacant Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_vacant" runat="server" Width="125px" CssClass="textbox ddlheight2 textbox1">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Filled</asp:ListItem>
                                        <asp:ListItem>Un Filled</asp:ListItem>
                                        <asp:ListItem>Partially Filled</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" Text="Include:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chtrhos" runat="server" Text="All" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnCheckedChanged="chtrhos_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="cbl_trhos" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_trhos_SelectedIndexChanged"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                        <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                        <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_trhosgo" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_trhosgo_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lblertrhos" runat="server" Style="color: Red;"></asp:Label></center>
                    <br />
                    <div id="div5" runat="server" style="width: 800px; height: 180px; overflow: auto">
                        <center>
                            <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" Style="width: 800px; height: 180px;
                                overflow: auto">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread></center>
                    </div>
                    <br />
                    <center>
                        <table class="maintablestyle" runat="server" id="tblStatusguest" style="border-bottom-style: solid;
                            border-top-style: solid; border-left-style: solid; border-width: 0px;" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="toalroomsguest" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="totalvaccantsguest" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%-- <asp:Button ID="Button4" runat="server" Width="20px" BackColor="GreenYellow" />--%>
                                    <asp:Label ID="Label2" runat="server" Width="20px" Height="20px" BackColor="GreenYellow"></asp:Label>
                                    <asp:Label ID="fillguest" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="109px"></asp:Label>
                                    <%-- <asp:Button ID="Button5" runat="server" Width="20px" BackColor="Coral" />--%>
                                    <asp:Label ID="Label3" runat="server" Width="20px" Height="20px" BackColor="Coral"></asp:Label>
                                    <asp:Label ID="partialfillguest" runat="server" Text="Partially Filled" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                    <%-- <asp:Button ID="Button6" runat="server" Width="20px" BackColor="MistyRose" />--%>
                                    <asp:Label ID="Label4" runat="server" Width="20px" Height="20px" BackColor="MistyRose"></asp:Label>
                                    <asp:Label ID="unfillguest" runat="server" Text="Unfilled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="145px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_trhos" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_trhos_Click" />
                            <asp:Button ID="btn_trhosexit" Text="Exit" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_trhosexit_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="Div4" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="imgsuspension" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 294px;"
                    OnClick="imgsuspension_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 609px;
                    height: 324px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Suspension Details"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 545px; height: 223px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Suspension"></asp:Label>
                                        <asp:CheckBox ID="cb_date" runat="server" OnCheckedChanged="cb_date_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:CalendarExtender ID="cext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1  txtheight"
                                            AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="cext_todate" TargetControlID="txt_todate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_susreason" runat="server" Text="Reason"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_susreason" runat="server" CssClass="textbox  ddlheight3"
                                            Style="float: left;" onchange="change2(this)" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_susreason" CssClass="textbox txtheight3 textbox1" Style="display: none;
                                            float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_rejoin" runat="server" Text="Rejoin"></asp:Label>
                                        <asp:CheckBox ID="cb_rejoin" runat="server" OnCheckedChanged="cb_rejoin_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_rejoindate" runat="server" Text="Rejoin Date"></asp:Label>
                                        <asp:TextBox ID="txt_rejoindate" runat="server" CssClass="textbox textbox1 txtheight"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_rejoindate" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_savesus" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_savesus_Click"
                                    OnClientClick="return validsus()" />
                                <asp:Button ID="btn_exitsus" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exitsus_Click" />
                            </div>
                        </center>
                    </div>
                </div>
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
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
</asp:Content>
