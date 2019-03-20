<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="GymAllotment.aspx.cs" Inherits="HostelMod_GymAllotment" %>

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


            function valid1() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                id = document.getElementById("<%=txt_pop1rollno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1rollno.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_studentname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_studentname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_degre.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_degre.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_cost.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_cost.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_date.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_date.ClientID %>");
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
            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                id = document.getElementById("<%=txt_pop1staffname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1staffname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_staffcode.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_staffcode.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_dept.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_dept.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_design.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_design.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_cost.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_cost.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_date.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_date.ClientID %>");
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
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000;">Gym Allotment</span>
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
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_roll" Text="Roll No" runat="server" CssClass="txtheight"></asp:Label>
                                    <asp:Label ID="lbl_satff_dept" Text="Department" runat="server" CssClass="txtheight"
                                        Visible="false"></asp:Label>
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
                                    <asp:UpdatePanel ID="upp_department" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_department" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_department" runat="server" Height="200px" Width="180px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_department" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_department_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_department" runat="server" TargetControlID="txt_department"
                                                PopupControlID="panel_department" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_name" Text="Name" runat="server" CssClass="txtheight2"></asp:Label>
                                    <asp:Label ID="lbl_designation" Text="Designation" runat="server" Visible="false"></asp:Label>
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
                                    <asp:UpdatePanel ID="upp_designation" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_designation" runat="server" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_designation" Height="200px" Width="180px" runat="server" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_designation" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_designation_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_designation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_designation_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_designation" runat="server" TargetControlID="txt_designation"
                                                PopupControlID="panel_designation" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr id="studdetails" runat="server">
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
                                    <asp:Label ID="lbl_stafftype" Text="Staff Type" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_vacate" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_vacate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <asp:UpdatePanel ID="upp_stafftype" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stafftype" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_stafftype" runat="server" Height="100px" Width="180px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_stafftype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_stafftype_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_stafftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftype_checkedchange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_stafftype" runat="server" TargetControlID="txt_stafftype"
                                                PopupControlID="panel_stafftype" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                                    <asp:Label ID="lbl_Gender" Text="Gender" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_Gender" runat="server">
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
                                    <asp:TextBox ID="txt_staffname" runat="server" placeholder="Staff Name" CssClass=" textbox textbox1 txtheight2"
                                        Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_staffname" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffNamego" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_community" Text="Community" Width="84px" runat="server"></asp:Label>
                                    <asp:Label ID="lblstaffcode" runat="server" Text="Staff Code" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp_community" runat="server">
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
                                    <asp:TextBox ID="txtstafcode" runat="server" placeholder="Staff Code" CssClass=" textbox textbox1 txtheight2"
                                        Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_staffcode" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstafcode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblgymname" Text="Gym Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp_gymname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_gymname" runat="server" CssClass="textbox txtheight1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="P10" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_gymname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_gymname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_gymname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_gymname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_gymname"
                                                PopupControlID="p10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblstudentstaff" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblstudentstaff_Selected">
                                        <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                        OnClick="btn_go_Click" Style="float: right;" BackColor="LightGreen" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_add" Text="Add New" CssClass="textbox btn2" runat="server" OnClick="btn_add_Click"
                                        BackColor="LightGreen" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <center>
                        <asp:Label ID="lbl_errmsg" runat="server" Style="color: Red;"></asp:Label>
                    </center>
                    <br />
                    <%-- Student ColumnOrder--%>
                    <div id="StudentColumnOrder" runat="server">
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
                                                 <asp:ListItem Selected="True" Value="id">Student Id</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="Roll_Admit">Admission No</asp:ListItem>
                                               
                                                <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="Degree">Course</asp:ListItem>
                                                <asp:ListItem Value="Reg_No">Reg No</asp:ListItem>
                                                <%--  <asp:ListItem Value="DOB">DOB</asp:ListItem>
                                            <asp:ListItem Value="Parent_AddressP">Address</asp:ListItem>
                                            <asp:ListItem Value="PAddress">Street</asp:ListItem>
                                            <asp:ListItem Value="CityP">City</asp:ListItem>
                                            <asp:ListItem Value="Ppincode">Pincode</asp:ListItem>
                                            <asp:ListItem Value="districtp">District</asp:ListItem>
                                            <asp:ListItem Value="StateP">State</asp:ListItem>--%>
                                                <asp:ListItem Value="community">Community</asp:ListItem>
                                                <asp:ListItem Value="region">Religion</asp:ListItem>
                                                <%--   <asp:ListItem Value="Admin_Date">Admit Date</asp:ListItem>--%>
                                                <asp:ListItem Value="BuildingFK">Building</asp:ListItem>
                                                <asp:ListItem Value="FloorFK">Floor</asp:ListItem>
                                                <asp:ListItem Value="RoomFK">Room</asp:ListItem>
                                                <asp:ListItem Value="Room_type">Room Type</asp:ListItem>
                                                <%-- <asp:ListItem Value="DiscontinueDate">Discontinue</asp:ListItem>--%>
                                                <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                                <%--  <asp:ListItem Value="Student_Mobile">Student Mobile</asp:ListItem>--%>
                                                <asp:ListItem Value="StudMessType">Student Mess Type</asp:ListItem>
                                                <asp:ListItem Value="GymName">Gym Name</asp:ListItem>
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
                    </div>
                    <%--Staff columnOrder--%>
                    <div id="StaffColumnOrder" runat="server" visible="false">
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="940px" Style="margin-top: -0.1%;">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="Labelfilter1" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter1" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <center>
                            <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="940px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column1_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="tborder1" Visible="false" Width="930px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                                Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder1_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="staff_code">Staff Code</asp:ListItem>
                                                 <asp:ListItem Selected="True" Value="id">Staff Id</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="staff_name">Name</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="desig_name">Designation</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="dept_name">Department</asp:ListItem>
                                                <asp:ListItem Value="staffcategory">Staff Type</asp:ListItem>
                                                <asp:ListItem Value="Admin_Date">Admit Date</asp:ListItem>
                                                <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                                <asp:ListItem Value="BuildingFK">Building</asp:ListItem>
                                                <asp:ListItem Value="FloorFK">Floor</asp:ListItem>
                                                <asp:ListItem Value="RoomFK">Room</asp:ListItem>
                                                <asp:ListItem Value="GymName">Gym Name</asp:ListItem>
                                                <%-- <asp:ListItem Value="DiscontinueDate">Discontinue</asp:ListItem>
                                            <asp:ListItem Value="VacatedDate">Vacated</asp:ListItem>
                                            <asp:ListItem Value="Reason">Reason</asp:ListItem>
                                            <asp:ListItem Value="StudMessType">StudMessType</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder1" runat="server" TargetControlID="pcolumnorder1"
                            CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                            TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </div>
                    <br />
                    <center>
                        <asp:Label Style="color: Red;" ID="lblerr" Text="" runat="server" Visible="false"></asp:Label>
                    </center>
                    <p style="width: 840px; margin-top: 0px;" align="right">
                        <asp:Label ID="lbl_stucnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <center>
                        <div id="div2" runat="server" visible="false" style="width: 923px; height: 350px;
                            background-color: White;" class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" OnCellClick="FpSpread1_CellClick"
                                OnPreRender="FpSpread1_SelectedIndexChanged">
                                <%--                               
                                 OnUpdateCommand="Fpspread1_Command"--%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="printdiv1" runat="server" visible="false" style="height: 100%; z-index: -1;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                        </div>
                        <div id="rptprint1" runat="server" visible="false">
                            <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname1" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname1" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel1_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                        </div>
                    </center>
                    <center>
                        <div id="div4" runat="server" visible="false" style="width: 923px; height: 350px;
                            background-color: White;" class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" OnCellClick="FpSpread2_CellClick"
                                OnPreRender="FpSpread2_SelectedIndexChanged">
                                <%--                               
                                 OnUpdateCommand="Fpspread1_Command"--%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="printdiv" runat="server" visible="false" style="height: 100%; z-index: -1;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                        </div>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popwindowaddnew" runat="server" class="popupstyle" visible="false" style="height: 61em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 234px;"
                        OnClick="imagebtnpop1close_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 543px; width: 500px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader ">Gym Allotment</span></div>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <fieldset style="width: 150px; height: 15px;">
                                            <asp:RadioButtonList ID="rblstustaff" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblstustaff_Selected">
                                                <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1hostelname" Text="Hostel Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop1hostelname" runat="server" CssClass="textbox ddlheight4 textbox1"
                                            Width="195px" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1rollno" Text="Roll No" runat="server"></asp:Label>
                                        <asp:Label ID="lbl_pop1staffname" Text="Staff Name" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1rollno" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px" AutoPostBack="true"
                                            OnTextChanged="roll_txtchange" MaxLength="30"></asp:TextBox>
                                        <asp:Button ID="btn1sturoll" Text="?" runat="server" OnClick="btnsturollno_Click"
                                            CssClass="textbox btn" />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_pop1staffname" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px" AutoPostBack="true"
                                            OnTextChanged="Staffname_txtchange" MaxLength="30" Visible="false"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:Button ID="btnstaffname" Text="?" runat="server" OnClick="btnstaffname_Click"
                                            CssClass="textbox btn" Visible="false" />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1staffname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                 <td>
                                        <asp:Label ID="Label1" Text="Student Id" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" Text="Staff Id" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px" 
                                            MaxLength="30"  ></asp:TextBox>
                                            <asp:TextBox ID="txtid1" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px" Visible="false"
                                            MaxLength="30"  ></asp:TextBox>
                                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_studentname" Text="Student Name" runat="server"></asp:Label>
                                        <asp:Label ID="lbl_staffcode" Text="Staff Code" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_studentname" runat="server" ReadOnly="true" BackColor="#DCF9D1"
                                            CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:TextBox ID="txt_staffcode" runat="server" ReadOnly="true" BackColor="#DCF9D1"
                                            CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degre" Text="Degree" runat="server"></asp:Label>
                                        <asp:Label ID="lbl_dept" Text="Department" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_degre" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true" Visible="false"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr id="staffdesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl_design" Text="Designation" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_design" runat="server" CssClass="textbox txtheight5 textbox1"
                                            BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_gymname" Text="Gym Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_gymname" runat="server" CssClass="textbox textbox1 ddlheight2"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_gymname_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cost" Text="Cost" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cost" runat="server" CssClass="textbox txtheight5 textbox1"
                                            Enabled="false"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                            Format="dd/MM/yyyy" Enabled="True">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr id="discontinue" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl_Discontinue" Text="Discontinue Reason" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                        <asp:DropDownList ID="ddl_discontinue" CssClass="ddlheight1 textbox1" runat="server">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <fieldset style="width: 300px; height: 15px;">
                                            <asp:RadioButtonList ID="rblCost_Wise" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblCost_Wise_Selected">
                                                <asp:ListItem Text="Sem" Value="0"  Enabled="false" Selected="false"></asp:ListItem>
                                                <asp:ListItem Text="Yearly" Value="1" Enabled="false"></asp:ListItem>
                                                <asp:ListItem Text="Monthly" Selected="True" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Term" Value="3" Enabled="false"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_Save_student" Text="Save" CssClass="textbox btn2" runat="server"
                                            OnClick="btn_Save_student_Click" BackColor="LightGreen" OnClientClick="return valid1()" />
                                        <asp:Button ID="btn_Save_Staff" Text="Save" CssClass="textbox btn2" runat="server"
                                            OnClick="btn_Save_Staff_Click" BackColor="LightGreen" OnClientClick="return valid2()"
                                            Visible="false" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_delete" Text="Delete" CssClass="textbox btn2" runat="server"
                                            OnClick="btn_delete_Click" BackColor="LightGreen" OnClientClick="return valid2()"
                                            Visible="false" />
                                        <asp:Button ID="btn_DisContinue" Text="DisContinue" CssClass="textbox btn2" runat="server"
                                            OnClick="btn_DisContinue_Click" BackColor="LightGreen" OnClientClick="return valid2()"
                                            Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
            </center>
        </div>
        <center>
            <div id="popwindowstudent" runat="server" class="popupstyle" visible="false" style="height: 50em;
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
                                <asp:Label ID="lbl_pop2batchyr" Text="Batch Year" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2batchyear_OnSelectedIndexChanged">
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
                                <asp:Label ID="lbl_pop2sex" Text="Gender" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2sex" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                    <asp:ListItem Value="2">Female</asp:ListItem>
                                    <asp:ListItem Value="3">Transgender</asp:ListItem>
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
                    <%-- <div style="width: 250px; float: right;">
                        <asp:Label ID="lblcounttxt" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                        <asp:Label ID="lblcount" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                    </div>--%>
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="fpsturoll" runat="server" Visible="false" Style="overflow: auto;
                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <asp:Button ID="btn_pop2ok" Text="Ok" runat="server" CssClass="textbox btn2" OnClick="btn_pop2ok_Click" />
                    <asp:Button ID="btn_pop2exit" Text="Exit" runat="server" CssClass="textbox btn2"
                        OnClick="btn_pop2exit_Click" />
                </div>
            </div>
        </center>
        <center>
            <div id="popupwindowstaff" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" CssClass="fontstyleheader" runat="server" Style="color: Green;"
                            Text="Select the Staff Name"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" style="width: 69%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegesatff" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegestaff" Width="250px" Height="30px" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_collegestaff_selectedindexchange"
                                            CssClass="textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_staffdepartment" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_staffdepartment" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight6" OnSelectedIndexChanged="ddl_staffdepartment_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchbystaff" Width="250px" Height="30px" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_searchbystaff_SelectedIndexChanged"
                                            CssClass="textbox1 ddlheight6">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_staffnamesearch" TextMode="SingleLine" runat="server" Height="20px"
                                            CssClass="textbox textbox1" Width="180px" placeholder="Staff Name"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_staffcodesearch" placeholder="Staff Code" Visible="false" TextMode="SingleLine"
                                            runat="server" Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCodepopup" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_staffselectgo" runat="server" CssClass="textbox btn1" Text="Go"
                                            OnClick="btn_staffselectgo_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <p>
                                    <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </p>
                                <p style="width: 691px;" align="right">
                                    <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                </p>
                                <div id="div1" runat="server" visible="false" style="width: 877px; height: 368px;
                                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <br />
                                    <center>
                                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Style="overflow: auto;
                                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                            box-shadow: 0px 0px 8px #999999;">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </div>
                            </div>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_staffok" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="OK" OnClick="btn_staffok_Click" />
                                    <asp:Button ID="btn_staffexit" runat="server" Visible="false" CssClass="textbox btn2"
                                        Text="Exit" OnClick="btn_staffexit_Click" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <div>
            <center>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
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
        </div>
        <center>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                        onkeypress="display1()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="line-height: 35px">
                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
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
                                                OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
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
