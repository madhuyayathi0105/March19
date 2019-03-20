<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true" CodeFile="CertificateMasterReport.aspx.cs" Inherits="CertificateMasterReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Certificate Issue Return</span></div>
           
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" style="width: 1000px; height: auto">
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlclg_SelectedIndexChanged" Width="217px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                  <asp:Label ID="lblStr" Text="Stream" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stream" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 100px; height: 130px;">
                                            <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_stream_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_stream"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Course Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddledu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddledu_OnSelectedIndexChanged"
                                    Style="width: 67px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Batch Year
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_OnSelectedIndexChanged"
                                    Style="width: 67px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                  <asp:Label ID="lbldegree" Text="Degree" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight"
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
                                <asp:Label ID="lbl_branch" Text="Department" runat="server"></asp:Label>
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
                                Certificate
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_certificate" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="200px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_certificate" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_certificate_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_certificate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_certificate_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_certificate"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Status
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_status" runat="server" CssClass="textbox textbox1 txtheight"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel9" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="200px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_statusdetail" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_statusdetail_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_status" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_status_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_status"
                                            PopupControlID="Panel9" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_cumm" Visible="true" runat="server" Text="Cumulative" GroupName="a"
                                    Checked="true" AutoPostBack="true" OnCheckedChanged="rdb_cumm_CheckedChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_detail" Visible="true" runat="server" Text="Details" GroupName="a"
                                    AutoPostBack="true" OnCheckedChanged="rdb_detail_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                    OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <br />
                        <asp:Label ID="lbl_err_stud" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                    </div>
                    <br />
                     <center>
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <div id="div_report1" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname1" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txt_excelname1" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname1_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_excelname1"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel1" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btn_Excel1_click" />
                            <asp:Button ID="btn_printmaster1" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btn_printmaster1_Click" />
                            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                        </center>
                    </div>
                    <br />
                    <br />
                    <asp:Label ID="lbl_headernamespd2" runat="server" ForeColor="Green" Visible="false"
                        Font-Size="X-Large"></asp:Label>
                    <asp:Label ID="Label1" Font-Bold="true" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                    <center>
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                            OnCellClick="FpSpread2_CellClick" OnPreRender="FpSpread2_SelectedIndexChanged"
                            OnButtonCommand="FpSpread2_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <div id="div_report" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btnExcel_Click" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btn_printmaster_Click" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </div>
                    <br />
                    <div id="popview" runat="server" class="popupstyle popupheight1" visible="false">
                        <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 471px;"
                            OnClick="btn_popclose_Click" />
                        <br />
                        <div style="background-color: White; height: 714px; width: 967px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <span class="fontstyleheader" style="color: Green;">Certification Issue and Return</span>
                            <br />
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collg" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" Width="217px" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpopStr" Text="Stream" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop_stream" runat="server" CssClass="textbox txtheight textbox1"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Style="width: 100px; height: 130px;">
                                                    <asp:CheckBox ID="cb_pop_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_pop_stream_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop_stream_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_pop_stream"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Course Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop_edu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_pop_edu_OnSelectedIndexChanged"
                                            Style="width: 67px; height: 30px;" CssClass="textbox3 textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Batch
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_pop_batch_OnSelectedIndexChanged"
                                            Style="width: 67px; height: 30px;" CssClass="textbox3 textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpopDeg" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop_degree" runat="server" CssClass="textbox  textbox1 txtheight"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" runat="server" BackColor="White" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px"
                                                    Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop_degree_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop_degree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_pop_degree"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop_branch" Text="Branch" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop_branch" runat="server" CssClass="textbox textbox1 txtheight4"
                                                    ReadOnly="true" Width="204px">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop_branch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_pop_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_pop_branch"
                                                    PopupControlID="Panel5" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Certificate
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop_certificate" runat="server" CssClass="textbox textbox1 txtheight"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop_certificate" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop_certificate_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop_certificate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop_certificate_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_pop_certificate"
                                                    PopupControlID="Panel6" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdo_received" runat="server" Text="Receive" GroupName="ss" Checked="true" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdo_Issue" runat="server" Text="Issused" GroupName="ss" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_pop_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_pop_go_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Search
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddl_rollno" runat="server" CssClass="textbox  ddlheight2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_rollno_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox textbox1 txtheight3"
                                            AutoPostBack="true" OnTextChanged="txt_rollno_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        Student Name
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_studname" runat="server" CssClass="textbox textbox1 txtheight4"
                                            AutoPostBack="true" OnTextChanged="txt_studname_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <br />
                                <asp:Label ID="lbl_pop_error" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                                <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <br />
                            </div>
                            <br />
                            <center>
                                <div id="div_save" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                Date
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_fromdate" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_satff" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_staffsearch" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxstaffsearch" runat="server" TargetControlID="txt_staffsearch"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom,Numbers" ValidChars=" .-">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="autostudindi1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffsearch"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_save" Visible="false" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                    OnClick="btn_save_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </div>
                    <br />
                    <div id="viewfile" runat="server" class="popupstyle popupheight1" visible="false">
                        <asp:ImageButton ID="imgfileviewclose" runat="server" Width="40px" Height="40px"
                            ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                            margin-top: 50px; margin-left: 432px;" OnClick="imgfileviewclose_Click" />
                        <br />
                        <div style="background-color: White; height: 542px; width: 886px; margin-top: 47px;
                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <span class="fontstyleheader" style="color: Green;">View Certificate Files</span>
                            <br />
                            <br />
                            <center>
                                <asp:Label ID="Label2" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                                <FarPoint:FpSpread ID="Fpspread4" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                                    OnButtonCommand="FpSpread4_ButtonCommand">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                            <br />
                            <center>
                                <asp:Button ID="btn_viewfileclose" Visible="false" runat="server" Text="Exit" OnClick="btn_viewfileclose_clik"
                                    CssClass="textbox1 textbox btn2" /></center>
                        </div>
                    </div>
                    <br />
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
                                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" Font-Bold="true" />
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
        </center>
    </div>
</asp:Content>

