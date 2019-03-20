<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HierarchySettingReport.aspx.cs" Inherits="HierarchySettingReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: #008000;">Request Hierarchy Report</span></div>
                <br />
                <div class="maindivstyle" style="width: 1000px; height: 1300px;">
                    <br />
                    <table class="maindivstyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_reqname" runat="server" Text="Request Name" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_reqname" runat="server" CssClass="textbox ddlheight4" OnSelectedIndexChanged="ddl_reqname_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <%--College Name--%>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollegestaff" Width="220px" runat="server" OnSelectedIndexChanged="ddlcollegestaff_OnSelectedIndexChanged" CssClass="textbox textbox1 ddlheight5">
                                </asp:DropDownList>
                            </td>
                            <%--Department Name--%>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UPdp_deprt" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstaffDepart" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel multxtpanleheight" Width="350px">
                                            <asp:CheckBox ID="chkdeptstaff" runat="server" OnCheckedChanged="chkdeptstaff_CheckedChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chldeptstaff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chldeptstaff_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtstaffDepart"
                                            PopupControlID="pnldept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--Staff Type--%>
                            <td>
                                <asp:Label ID="lblstafftype_new" runat="server" Text="Staff Type" Font-Bold="True"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstaff_type" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlstafftype" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chkstafftypenew" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chkstafftypenew_CheckedChanged" />
                                            <asp:CheckBoxList ID="chlstafftpyenew" runat="server" AutoPostBack="true" Font-Size="Medium"
                                                OnSelectedIndexChanged="chlstafftpyenew_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtstaff_type"
                                            PopupControlID="pnlstafftype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--Designation--%>
                            <td>
                                <asp:Label ID="lblstaff" runat="server" Text="Designation" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstaff" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">---Select---</asp:TextBox>
                                        <asp:Panel ID="pstaff" runat="server" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chksatff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chksatff_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklststaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklststaff_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstaff"
                                            PopupControlID="pstaff" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_maingo" runat="server" Text="Go" OnClick="btn_maingo_Click" CssClass="btn1 textbox textbox1" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <asp:Label ID="lbl_error_shown" Visible="false" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label></center>
                    <br />
                    <div style="overflow: auto;">
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread1_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <center>
                        <div id="div_report" runat="server" visible="false">
                            <center>
                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                    CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_Excel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2" AutoPostBack="true"
                                    OnClick="btnExcel_Click" />
                                <asp:Button ID="btn_printmaster" Font-Bold="True" Font-Names="Book Antiqua" runat="server"
                                    Text="Print" CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </center>
                        </div>
                    </center>
                    <center>
                        <div id="popview" runat="server" style="height: 100%; z-index: 1000; width: 100%;
                            background-color: rgba(54, 25, 25, .2); position: absolute; top: 102px; left: 0px;"
                            visible="false">
                            <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 417px;"
                                OnClick="btn_popclose_Click" />
                            <br />
                            <div style="background-color: White; height: 570px; width: 860px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <span class="fontstyleheader" style="color: #008000;">Update Staff</span>
                                <br />
                                <br />
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_poupcollege" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_poupdept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_staff_dept11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                        onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                        <asp:CheckBox ID="cb_staff_dept11" runat="server" Width="100px" Text="Select All"
                                                            OnCheckedChanged="cb_staff_dept11_CheckedChanged" AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_staff_dept11" runat="server" OnSelectedIndexChanged="cbl_staff_dept11_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_staff_dept11"
                                                        PopupControlID="panel7" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_popupstafftype" runat="server" Text="Staff Type">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_staff_type11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                        onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                        <asp:CheckBox ID="cb_staff_type111" runat="server" Width="100px" Text="Select All"
                                                            OnCheckedChanged="cb_staff_type111_CheckedChanged" AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_staff_type111" runat="server" OnSelectedIndexChanged="cb_staff_type111_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_staff_type11"
                                                        PopupControlID="panel8" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_popdesign" runat="server" Text="Designation"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_staff_desg111" runat="server" CssClass="textbox txtheight2"
                                                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel10" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                        <asp:CheckBox ID="cb_staff_desn11" runat="server" Width="100px" Text="Select All"
                                                            OnCheckedChanged="cb_staff_desn11_CheckedChanged" AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_staff_desn11" runat="server" OnSelectedIndexChanged="cbl_staff_desn11_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txt_staff_desg111"
                                                        PopupControlID="panel10" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstaff" runat="server" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                                AutoPostBack="true" Visible="true" CssClass="ddlheight4 textbox textbox1">
                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                AutoPostBack="True" Visible="true" CssClass="txtheight3 textbox textbox1"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_search1" runat="server" OnTextChanged="txt_search1_TextChanged"
                                                AutoPostBack="True" Visible="false" CssClass="txtheight3 textbox textbox1"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <td>
                                                <asp:Button ID="btn_gostaff" runat="server" CssClass="textbox1 textbox btn1" Text="Go"
                                                    OnClick="btn_gostaff_Click" />
                                            </td>
                                    </tr>
                                </table>
                                <br />
                                <div style="margin-left: 377px;">
                                    <asp:Label ID="lbl_totalstaffcount" runat="server" ForeColor="Green"></asp:Label></div>
                                <br />
                                <center>
                                    <asp:Label ID="ermsg" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                        Width="510" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                                        BorderStyle="Double" OnCellClick="fsstaff_CellClick">
                                        <CommandBar BackColor="Control" ButtonType="PushButton" Visible="false">
                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="CadetBlue">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                                <br />
                                <center>
                                    <asp:Button runat="server" ID="btnstaffadd" CssClass="btn2 textbox textbox1" Text="Update"
                                        OnClick="btnstaffadd_Click" />
                                    <asp:Button runat="server" ID="btnexitpop" Text="Exit" CssClass="btn1 textbox textbox1"
                                        OnClick="exitpop_Click" />
                                </center>
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
            </center>
        </div>
    </div>
</asp:Content>
