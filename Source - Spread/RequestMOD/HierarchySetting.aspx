<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HierarchySetting.aspx.cs" Inherits="HierarchySetting" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <body>
        <form id="form1">
        <asp:UpdatePanel ID="upp" runat="server">
            <ContentTemplate>
                <div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <br />
                    <center>
                        <div>
                            <center>
                                <div>
                                    <span class="fontstyleheader" style="color: #008000;">Request Hierarchy</span></div>
                                <br />
                            </center>
                        </div>
                        <div class="maindivstyle" style="width: 1035px; height: 1300px;">
                            <br />
                            <table class="maindivstyle" style="margin-top: 2px; height: 36px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_reqname" runat="server" Text="Request Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_reqname" runat="server" CssClass="textbox ddlheight4" OnSelectedIndexChanged="ddl_reqname_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblStaffStudent" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblStaffStudent_SelectedIndexChanged"
                                            Style="display: none;">
                                            <asp:ListItem Selected="True">Student</asp:ListItem>
                                            <asp:ListItem>Staff</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <div id="div_gatepass" runat="server" visible="false" style="margin-left: 517px;
                                width: 430px; margin-top: -45px">
                                <table class="maindivstyle" style="margin-left: -82px; margin-top: 5px; height: 36px">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdo_req_stud" runat="server" GroupName="req" Text="Student"
                                                Checked="true" AutoPostBack="true" OnCheckedChanged="rdo_req_stud_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdo_staff_req" runat="server" GroupName="req" Text="Staff" AutoPostBack="true"
                                                OnCheckedChanged="rdo_staff_req_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="tbl_stud" runat="server" class="maindivstyle" style="margin-left: 252px;
                                    margin-top: -37px; height: 36px; width: auto;">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdo_gatepass_staff" runat="server" GroupName="gate1" Checked="true"
                                                Text="Staff" AutoPostBack="true" OnCheckedChanged="rdo_gatepass_staff_CheckedChange" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdo_gatepass_dept" Checked="false" runat="server" GroupName="gate1"
                                                Text="Department" AutoPostBack="true" OnCheckedChanged="rdo_gatepass_dept_CheckedChange" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbreqstu" Checked="false" runat="server" GroupName="gate1"
                                                Text="Request For Student" AutoPostBack="true" OnCheckedChanged="rdbreqstu_CheckedChange" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div_hostel" runat="server" visible="false" style="margin-left: 517px; width: 430px;
                                margin-top: -45px">
                                <table id="hostevat" runat="server" class="maindivstyle" style="margin-left: 20px;
                                    margin-top: -37px; height: 36px; width: auto;" visible="false">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_hostelstd" Checked="true" runat="server" GroupName="gate1"
                                                Text="RequestForStudent" AutoPostBack="true" OnCheckedChanged="rdb_hostelstd_CheckedChange" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div id="divStaffDet" runat="server">
                                <div id="divStaffDept" runat="server" visible="false">
                                    <center>
                                        <table id="dept" runat="server" class="maintablestyle">
                                            <tr>
                                                <%--College Name--%>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                        OnSelectedIndexChanged="ddlcol_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Lbel7" runat="server" Text="Batch Year" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtbat" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true">---Select---</asp:TextBox>
                                                            <asp:Panel ID="Panel2" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="ChklBatch" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="ChkBatch_SelectedIndexChanged" />
                                                                <asp:CheckBoxList ID="Cbllbatch" runat="server" AutoPostBack="true" Font-Size="Medium"
                                                                    OnSelectedIndexChanged="Cblbatch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtbat"
                                                                PopupControlID="Panel2" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <%--Designation--%>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight3 textbox1"
                                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                                                PopupControlID="p1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <%--Department Name--%>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Branch " Visible="false" Font-Bold="True"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" Visible="false">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox txtheight3 textbox1"
                                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_branch"
                                                                PopupControlID="Panel1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <%--Staff Type--%>
                                                <td>
                                                    <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Upp2hostel" runat="server" Visible="false">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_hostelname_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_hostelname"
                                                                PopupControlID="Panel3" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngostu" runat="server" Text="Go" OnClick="btngostu_Click" CssClass="btn1 textbox textbox1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                                <div id="divStaffDets" runat="server">
                                    <center>
                                        <table class="maintablestyle" id="tbstaff" runat="server">
                                            <tr>
                                                <%--College Name--%>
                                                <td>
                                                    <asp:Label ID="Label13" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcollegestaff" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                        OnSelectedIndexChanged="ddlcollegestaff_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <%--Department Name--%>
                                                <td>
                                                    <asp:Label ID="lbldept" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
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
                                                    <asp:Label ID="lblstafftype_new" runat="server" Text="Staff Type" Font-Bold="true"
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
                                                    <asp:Label ID="lblstaff" runat="server" Text="Designation" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
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
                                                    <asp:Button ID="btnMainGo" runat="server" Text="Go" OnClick="btnMainGo_Click" CssClass="btn1 textbox textbox1" />
                                                    <asp:Button ID="btnMainGogatepass" Visible="false" runat="server" Text="Go" OnClick="btnMainGogatepass_Click"
                                                        CssClass="btn1 textbox textbox1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                                <center>
                                    <br />
                                    <asp:Label ID="lblerrstaff" runat="server" Text="" Visible="false" ForeColor="Red"
                                        CssClass="font" Font-Bold="true" Font-Size="Medium"></asp:Label></center>
                                <%--Fpspread2 for request--%>
                                <center>
                                    <table style="width: 395px; height: 182px;">
                                        <tr>
                                            <td>
                                                <center>
                                                    <br />
                                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" OnButtonCommand="FpSpread2_ButtonCommand"
                                                        OnUpdateCommand="FpSpread2_UpdateCommand" OnCellClick="FpSpread2_CellClick" OnPreRender="FpSpread2_SelectedIndexChanged"
                                                        Width="494" Height="117">
                                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                            ButtonShadowColor="ControlDark">
                                                        </CommandBar>
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 520px;
                                    height: 35px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Text="Range :"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                                                    MaxLength="4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_frange"
                                                    FilterType="Numbers" ValidChars="/">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_trange" CssClass="textbox textbox1 txtheight" runat="server"
                                                    MaxLength="4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_trange"
                                                    FilterType="Numbers" ValidChars="/">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="Btn_range" runat="server" Text="Select" OnClick="Btn_range_Click"
                                                    CssClass="textbox1 textbox btn2" Font-Bold="true" Font-Names="Book Antiqua" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="divStudSDet" runat="server">
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlclgStud" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlclgStud_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                                                AssociatedControlID="txtBatch"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="position: relative;">
                                                <asp:UpdatePanel ID="upnlBatch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                            ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                            Width="140px">
                                                            <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                                            PopupControlID="pnlBatch" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                                                AssociatedControlID="txtDegree"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="position: relative;">
                                                <asp:UpdatePanel ID="upnlDegree" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                            ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                            Width="140px">
                                                            <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                                            PopupControlID="pnlDegree" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                                                AssociatedControlID="txtBranch"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="position: relative;">
                                                <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                            ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                            Width="280px">
                                                            <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                            PopupControlID="pnlBranch" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBr" runat="server" Text="Branch"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                        PopupControlID="panel_dept" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btngo" runat="server" Height="30px" Width="40px" CssClass="textbox textbox1 btn3"
                                                Text="Go" OnClick="btngo_Click" />
                                            <%-- <asp:Button ID="btnadd" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                    Text="Add" OnClick="btnadd_Click" />--%>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="divspread" runat="server" visible="false" style="height: 400px; overflow: auto;">
                                    <FarPoint:FpSpread ID="fpreport" runat="server" Visible="true" BorderStyle="Solid"
                                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder" ShowHeaderSelection="false" OnCellClick="fpreport_OnCellClick"
                                        OnUpdateCommand="fpreport_Command" OnPreRender="fpreport_Selectedindexchanged">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <asp:Label ID="lblNoRec" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    ForeColor="Red" Text="No Records Found" Visible="false"></asp:Label>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                                    AutoPostBack="True" CssClass="textbox ddlheight ddlheight1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblstaffDept" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtstaffDept" runat="server" Height="20px" CssClass="textbox textbox1 txtheight1"
                                                            ReadOnly="true">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pstaffDept" runat="server" Height="240px" Style="text-align: left;"
                                                            CssClass="multxtpanel multxtpanleheight1">
                                                            <asp:CheckBox ID="chksatffDept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" OnCheckedChanged="chksatffDept_CheckedChanged" Text="Select All"
                                                                AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chklststaffDept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="chklststaffDept_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffDept"
                                                            PopupControlID="pstaffDept" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblstafftype" runat="server" Text="Staff Type" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtstaffType" runat="server" CssClass="textbox textbox1 txtheight1"
                                                            ReadOnly="true">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pstaffType" runat="server" Height="130px" Style="text-align: left;"
                                                            CssClass="multxtpanel">
                                                            <asp:CheckBox ID="chksatffType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" OnCheckedChanged="chksatffType_CheckedChanged" Text="Select All"
                                                                AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chklststaffType" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="chklststaffType_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtstaffType"
                                                            PopupControlID="pstaffType" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblstaffdeg" runat="server" Text="Designation" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtstaffDeg" runat="server" Height="20px" CssClass="textbox textbox1 txtheight1"
                                                            ReadOnly="true">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pnlstaffdeg" runat="server" Height="240px" Style="text-align: left;"
                                                            CssClass="multxtpanel multxtpanleheight1">
                                                            <asp:CheckBox ID="chkstaffdeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" OnCheckedChanged="chkstaffdeg_CheckedChanged" Text="Select All"
                                                                AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chlstaffdeg" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="chlstaffdeg_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtstaffDeg"
                                                            PopupControlID="pnlstaffdeg" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_criteria" runat="server" Text="Criteria" Font-Bold="true" Font-Names="Book Antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_criteria" runat="server" CssClass="textbox textbox1 txtheight"
                                                    MaxLength="1">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_criteria"
                                                    FilterType="Numbers" ValidChars="">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td colspan="3" runat="server">
                                                <asp:CheckBox ID="Chktutor" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Tutor" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txttutor" runat="server" Visible="false" CssClass="textbox textbox1 txtheight"
                                                    MaxLength="1">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txttutor"
                                                    FilterType="Numbers" ValidChars="">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStaffType" runat="server" Visible="false" OnSelectedIndexChanged="ddlStaffType_SelectedIndexChanged"
                                                    AutoPostBack="True" CssClass="textbox ddlheight ddlheight1">
                                                    <asp:ListItem>General</asp:ListItem>
                                                    <asp:ListItem>Staff Selector</asp:ListItem>
                                                    <asp:ListItem>Include Staff Selector</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo1_Click"
                                                    CssClass="btn1 textbox textbox1" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div id="tbl_div" runat="server" visible="false">
                                        <table class="maindivstyle">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btn_criteria1" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 1" OnClick="btn_criteria1_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria2" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 2" OnClick="btn_criteria2_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria3" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 3" OnClick="btn_criteria3_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria4" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 4" OnClick="btn_criteria4_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria5" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 5" OnClick="btn_criteria5_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria6" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 6" OnClick="btn_criteria6_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria7" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 7" OnClick="btn_criteria7_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria8" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 8" OnClick="btn_criteria8_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_criteria9" runat="server" Visible="false" Enabled="false" CssClass="textbox textbox1 btn2"
                                                        Text="Stage 9" OnClick="btn_criteria9_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </center>
                            <center>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true" Font-Size="Medium"
                                    Visible="false" ForeColor="Red" CssClass="font"></asp:Label>
                            </center>
                            <center>
                                <table style="width: 395px; height: 250px;">
                                    <tr>
                                        <td>
                                            <center>
                                                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                    OnButtonCommand="FpSpread1_ButtonCommand" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                                    BorderWidth="1px" Width="494" Height="157" HorizontalScrollBarPolicy="Always"
                                                    VerticalScrollBarPolicy="Always">
                                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                        ButtonShadowColor="ControlDark">
                                                    </CommandBar>
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <div class="maindivstyle" align="center" style="border-radius: 7px; width: 380px;
                                margin-left: 568px; height: 40px;">
                                <asp:Label ID="lblerrordisplay" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                    Font-Size="8pt" Font-Bold="true" Visible="false"></asp:Label>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="btn2 textbox textbox1"
                                                OnClick="btnreset_Click" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="Buttonsave" runat="server" Text="Save" CssClass="btn2 textbox textbox1"
                                                OnClick="Buttonsave_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnview" runat="server" Text="View" CssClass="btn2 textbox textbox1"
                                                OnClick="btnview_Click" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="btn2 textbox textbox1"
                                                OnClick="BtnNew_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <%--************--%>
                        <div id="imgdivalt" runat="server" visible="false" style="height: 150em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    margin-top: 1104px; border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_erroralert" runat="server" Style="color: Red;" Font-Bold="true"
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
                        <div id="alert_div" runat="server" visible="false" style="height: 150em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 284px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_alertt" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnerrclose1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                    <%-- ************--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </body>
    </html>
</asp:Content>
