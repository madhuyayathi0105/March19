<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Smartcard_Mapping.aspx.cs"
    Inherits="Smartcard_Mapping" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Smartcard_Mapping.aspx.cs" Inherits="Smartcard_Mapping" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblprinterr.ClientID %>').innerHTML = "";
            }

            $('html').bind('keypress', function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Smart Card Mapping</span>
                        <br />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdateP" runat="server">
                <ContentTemplate>
                    <div class="maindivstyle" style="width: 950px; height: auto;">
                        <center>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:RadioButton ID="rdo_stud1" runat="server" Text="Student" GroupName="ss" AutoPostBack="true"
                                        OnCheckedChanged="rdo_stud1_onchecked" />
                                    <asp:RadioButton ID="rdo_staff2" runat="server" Text="Staff" GroupName="ss" AutoPostBack="true"
                                        OnCheckedChanged="rdo_staff2_onchecked" />
                                </div>
                            </center>
                            <br />
                            <table class="maintablestyle" id="studtbl" runat="server" visible="false">
                                <tr>
                                    <td>
                                        College
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_maincol" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_maincol_selectchanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                            CssClass="textbox  ddlheight" Style="width: 108px;">
                                        </asp:DropDownList>
                                        <%--<asp:UpdatePanel ID="upd_stream" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stream" Style="height: 20px; width: 100px;" CssClass="textbox textbox1 txtheight1"
                                        runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_str" runat="server" CssClass="multxtpanel" Height="100px">
                                        <asp:CheckBox ID="cb_str" runat="server" Text="Select All" OnCheckedChanged="cb_str_changed"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_str" runat="server" OnSelectedIndexChanged="cbl_str_selected"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pcb_stream" runat="server" TargetControlID="txt_stream"
                                        PopupControlID="pnl_str" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        Batch
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upd_batch" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_batch" Style="height: 20px; width: 100px;" CssClass="textbox textbox1 txtheight1"
                                                    runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Height="200px">
                                                    <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" OnCheckedChanged="cb_batch_changed"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_batch" runat="server" OnSelectedIndexChanged="cbl_batch_selected"
                                                        AutoPostBack="True">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pcb_batch" runat="server" TargetControlID="txt_batch"
                                                    PopupControlID="Pfyear" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Degree
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UP_degree" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="height: 150px;">
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
                                </tr>
                                <tr>
                                    <td>
                                        Department
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Up_dept" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    Width="125px" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="height: 150px;">
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
                                    <td>
                                        Semester
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_sem" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_seme" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pcb_sem" runat="server" TargetControlID="txt_seme"
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
                                                <asp:TextBox ID="txt_sect" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="height: 150px;">
                                                    <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pcb_sec" runat="server" TargetControlID="txt_sect"
                                                    PopupControlID="panel_sect" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btngo" runat="server" CssClass="textbox textbox1 btn2" Text="Go"
                                            OnClick="btngo_click" />
                                        <asp:Button ID="btnaddnew" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                            OnClick="btnaddnew_click" />
                                    </td>
                                </tr>
                            </table>
                            <table class="maintablestyle" id="stafftbl" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_staffclg" runat="server" CssClass="textbox textbox1 ddlheight2"
                                            Width="240px" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department" Width="100px" Text="Department" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="uup1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_department" runat="server" CssClass="textbox textbox1" Width="120px"
                                                    Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pp0" runat="server" Width="200px" Height="250px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_department" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_department_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pup1" runat="server" TargetControlID="txt_department"
                                                    PopupControlID="pp0" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_design" Text="Designation" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="uup2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_design" runat="server" CssClass="textbox textbox1" Width="120px"
                                                    Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pp2" Height="200px" Width="180px" runat="server" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_design" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_desig_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_design" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_desig_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_design"
                                                    PopupControlID="pp2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstaff" Text="Staff Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="uup3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stafftype" runat="server" CssClass="textbox textbox1" Width="120px"
                                                    Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pp3" runat="server" Height="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_stafftype" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_stafftype_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_stafftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftype_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_stafftype"
                                                    PopupControlID="pp3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="Button1" runat="server" CssClass="textbox textbox1 btn2" Text="Go"
                                            OnClick="btngo_click" />
                                        <asp:Button ID="Button2" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                            OnClick="btnaddnew_click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lbl_err" runat="server" Text="" Font-Bold="true" Style="color: Red;"></asp:Label>
                            <br />
                            <center>
                                <div id="div1" runat="server" visible="false" style="width: 900px;">
                                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="750px" Style="overflow: auto; background-color: White;"
                                        OnCellClick="Cell_Click" OnPreRender="Fpspread1_render" OnButtonCommand="FpSpread1_ButtonCommand"
                                        ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <br />
                            <asp:Label ID="lbl_sprerr" runat="server" Text="" Font-Bold="true" Style="color: Red;"></asp:Label>
                            <br />
                            <center>
                                <div id="div2" runat="server" visible="false" style="width: 900px;">
                                    <FarPoint:FpSpread ID="Fpspread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="750px" Style="overflow: auto; background-color: White;"
                                        OnCellClick="Cellcont_Click" OnPreRender="Fpspread2_render" OnButtonCommand="Fpspread2_ButtonCommand"
                                        ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <center>
                                <div id="rptprint" runat="server" visible="false">
                                    <asp:Label ID="lblprinterr" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                        Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight3"
                                        onkeypress="display()"></asp:TextBox>
                                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                        Text="Export To Excel" Width="130px" />
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        CssClass="textbox textbox1 btn2" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </div>
                            </center>
                            <br />
                            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 15px; margin-left: 412px;"
                                    OnClick="imagebtnpopclose_Click" />
                                <br />
                                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 865px;
                                    height: 600px;" align="center">
                                    <br />
                                    <br />
                                    <fieldset style="width: 400px; height: 460px; border-radius: 15px; float: left;">
                                        <div id="div_roll" runat="server">
                                            <div style="width: 300px;">
                                                <br />
                                                <br />
                                                <center>
                                                    <table cellspacing="6">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:RadioButton ID="rdo_stud" runat="server" Text="Student" GroupName="s" AutoPostBack="true"
                                                                    OnCheckedChanged="rdo_stud_onchecked" />
                                                                <asp:RadioButton ID="rdo_staff" runat="server" Text="Staff" GroupName="s" AutoPostBack="true"
                                                                    OnCheckedChanged="rdo_staff_onchecked" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                College
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                                    OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox1 ddlheight" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddl_staffcode" Visible="false" runat="server" CssClass="textbox1 ddlheight1">
                                                                    <asp:ListItem Value="0">Staff Code</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_rerollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight4 textbox1"
                                                                    OnTextChanged="txt_rerollno_TextChanged"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_rerollno"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:TextBox ID="txt_staffcode" Visible="false" runat="server" placeholder="Staff code"
                                                                    AutoPostBack="true" CssClass="textbox txtheight4 textbox1" OnTextChanged="txt_staffcode_TextChanged"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_staffcode"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rerollno"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcode"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="TextBox4" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                                    onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                                    onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                                                <span style="color: Red;">*</span> <span id="Span2"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                SmartCard No
                                                                <asp:TextBox ID="txt_smart" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                    TextMode="Password"></asp:TextBox><%--OnTextChanged="txt_smart_change" AutoPostBack="true" --%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                Biometric UserID
                                                                <asp:TextBox ID="TextBioUserid" runat="server" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset style="width: 400px; height: 460px; border-radius: 15px; float: right;">
                                        <div id="div_refund" runat="server">
                                            <div style="width: 300px">
                                                <br />
                                                <br />
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Date
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_rdate" runat="server" ReadOnly="true" CssClass="textbox txtheight textbox1"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_rdate" runat="server"
                                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                        </asp:CalendarExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Name
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txt_rename" runat="server" ReadOnly="true" CssClass="textbox txtheight4 textbox1"
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
                                                                <asp:TextBox ID="txt_rebatch" runat="server" ReadOnly="true" CssClass="txtheight textbox textbox1">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Degree
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_redegree" runat="server" ReadOnly="true" CssClass="txtheight textbox textbox1">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Department
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txt_redept" runat="server" ReadOnly="true" CssClass="txtheight4 textbox textbox1">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_sem" runat="server" ReadOnly="true" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_str" runat="server" Text=""></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_restrm" runat="server" ReadOnly="true" CssClass="txtheight1 textbox textbox1">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Section
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_resec" runat="server" ReadOnly="true" CssClass="txtheight textbox textbox1">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="image3" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 130px;
                                                                    width: 100px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </div>
                                        </div>
                                        <br />
                                        <table id="stafftbl_det" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    Staff Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server">Staff Code</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staff_code" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server">Staff Type</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studenttype" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_desig" runat="server"> Designation</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_desig" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="4">
                                                    <asp:Image ID="lbl_studimage" runat="server" Width="130px" Height="120px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <br />
                                    <center>
                                        <br />
                                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                        <br />
                                        <div>
                                            <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" CssClass="textbox textbox1 btn2"
                                                Text="Save" />
                                            <asp:Button ID="btncancel" runat="server" OnClick="btncancel_Click" CssClass="textbox textbox1 btn2"
                                                Text="Cancel" />
                                        </div>
                                    </center>
                                </div>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
