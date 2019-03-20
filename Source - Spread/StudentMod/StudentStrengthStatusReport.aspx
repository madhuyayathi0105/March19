<%@ Page Title="Student Strength Status Report" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentStrengthStatusReport.aspx.cs" Inherits="StudentMod_StudentStrengthStatusReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Student Strength Status Report</title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 12000px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .newtextbox
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
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <script type="text/javascript">
        function ClearPrint() {
            var id = document.getElementById('<%=lblvalidation1.ClientID%>');
            id.innerHTML = "";
            id.visible = false;
        }
        function ClearPrint1() {
            var id = document.getElementById('<%=lbl_norec.ClientID%>');
            id.innerHTML = "";
            id.visible = false;
        }
    </script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Student Strength
                            Status Report</span>
                    </div>
                </center>
                <br />
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <%--<table class="maintablestyle" style="margin-left:-814px">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdo_applied" runat="server" GroupName="j" Text="Applied" Checked="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdo_admited" runat="server" GroupName="j" Text="Admitted" />
                            </td>
                        </tr>
                    </table>--%>
                    <center>
                        <table class="maintablestyle" width="800px">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Stream" Width="125px" runat="server" Text="Stream"></asp:Label>
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
                                    <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_graduation" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 130px; height: 130px;">
                                                <asp:CheckBox ID="cb_graduation" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_graduation_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_graduation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_graduation_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_graduation"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" Width="102px" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" Width="70px" CssClass="textbox txtheight1 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 120px; height: 150px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
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
                                    <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sec" runat="server" Width="70px" CssClass="textbox textbox1 txtheight"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                                PopupControlID="Panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_status" Visible="false" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_status_CheckedChanged" Checked="true" />
                                    <asp:Label ID="lbl_status" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_status" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel9" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="180px" Height="250px" Style="position: absolute;">
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
                                    <asp:DropDownList ID="ddl_status" runat="server" Visible="true" CssClass="ddlheight3 textbox1 textbox"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_from" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_from_CheckedChanged" Checked="false" />
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_fromdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_todate" Enabled="false" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
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
                            </tr>
                            <tr>
                                <td style="width: 250px" runat="server" id="tdlblstudtype" visible="false">
                                    <asp:Button ID="btn_go" Visible="false" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="Go" CssClass="textbox btn1 textbox1" OnClick="btn_go_Click" />
                                    <asp:CheckBox ID="cb_studtypechk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_studtypechk_CheckedChanged" />
                                    <asp:Label ID="lbl_studtype" Text="Student Type" runat="server" Style="float: right"></asp:Label>
                                </td>
                                <td runat="server" id="tdstudetype" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studtype" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_studtype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_studtype_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_studtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_studtype_SelectedIndexChanged">
                                                    <%--  <asp:ListItem Value="Hostler','Day Scholar">Both</asp:ListItem>
                                                <asp:ListItem Value="Hostler">Hostler</asp:ListItem>
                                                <asp:ListItem Value="Day Scholar">Day Scholar</asp:ListItem>--%>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_studtype"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 160px" runat="server" id="tdseattype" visible="false">
                                    <asp:CheckBox ID="cb_seatchk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_seatchk_CheckedChanged" />
                                    <asp:Label ID="lbl_seat" Text="Seat Type" runat="server" Style="float: left"></asp:Label>
                                </td>
                                <td runat="server" id="tdseattype1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_seat" runat="server" CssClass="textbox textbox1 txtheight" ReadOnly="true"
                                                Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_seat_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdtype" visible="false">
                                    <asp:CheckBox ID="cb_typechk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_typechk_CheckedChanged" />
                                    <asp:Label ID="lbl_type" Text="Student Type" Width="80px" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdtype1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_type" runat="server" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_type" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_type_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_type"
                                                PopupControlID="Panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdrelichk" visible="false">
                                    <asp:CheckBox ID="cb_relichk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_relichk_CheckedChanged" />
                                    <asp:Label ID="lbl_religion" Text="Religion" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdrelichk1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_religion" runat="server" CssClass="textbox textbox1 txtheight"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_religion" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_religion_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_religion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_religion"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td runat="server" id="tdcommchk" visible="false">
                                    <asp:CheckBox ID="cb_commchk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_commchk_CheckedChanged" />
                                    <asp:Label ID="lbl_comm" Text="community" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdcommchk1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_comm" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_comm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_comm_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_comm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_comm"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdresident" visible="false">
                                    <asp:CheckBox ID="cb_resident" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_resident_CheckedChanged" />
                                    <asp:Label ID="lbl_resident" Text="Residency" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdresident1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_resident" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel10" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="160px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_residency" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_residency_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_residency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_residency_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_resident"
                                                PopupControlID="Panel10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdsports" visible="false">
                                    <asp:CheckBox ID="cb_sports" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_sports_CheckedChanged" />
                                    <asp:Label ID="lbl_sports" Text="Sports" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdsports1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sports" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel12" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="160px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sport" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_sport_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sport_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_sports"
                                                PopupControlID="Panel12" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdlang" visible="false">
                                    <asp:CheckBox ID="cb_lang" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_lang_CheckedChanged" />
                                    <asp:Label ID="lbl_lang" Text="Language" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdlang1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_lang" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel13" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_language" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_language_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_language" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_language_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txt_lang"
                                                PopupControlID="Panel13" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" runat="server" id="tdmothertng" visible="false">
                                    <asp:CheckBox ID="cb_mothertng" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_mothertng_CheckedChanged" />
                                    <asp:Label ID="lbl_mothertng" Text="MotherTongue" Style="float: left" runat="server"></asp:Label>
                                    <%--</td>
                                <td>--%>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_mothertng" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true" Width="130px" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel14" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_mothertongue" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_mothertongue_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_mothertongue" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mothertongue_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txt_mothertng"
                                                PopupControlID="Panel14" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2" runat="server" id="tdphychallange" visible="false">
                                    <asp:CheckBox ID="cb_phychallange" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_phychallange_CheckedChanged" />
                                    <asp:Label ID="lbl_phychallange" Text="PhysicalChallange" Style="float: left" runat="server"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_phychallage" Style="float: left" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="115px" ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel15" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_phychlg" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_phychlg_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_phychlg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_phychlg_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_phychallage"
                                                PopupControlID="Panel15" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2" runat="server" id="tdtransport" visible="false">
                                    <asp:CheckBox ID="cb_trans" Enabled="false" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_trans_CheckedChanged" />
                                    <asp:Label ID="lbl_transport" Text="Transport Type" Style="float: left" runat="server"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_transport" Style="float: left" runat="server" CssClass="textbox textbox1 "
                                                Width="108px" ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel16" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_transport" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_transport_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_transport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_transport_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_transport"
                                                PopupControlID="Panel16" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_Gender" Visible="false" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_EnGender_CheckedChanged" />
                                    <asp:Label ID="lbl_gen" Visible="false" Text="Gender" Style="float: left" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_gen" Visible="false" Enabled="false" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel17" Visible="false" runat="server" BackColor="White" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_gen" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_Gender_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_gen" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_gen_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Male</asp:ListItem>
                                                    <asp:ListItem Value="1">Female</asp:ListItem>
                                                    <%-- <asp:ListItem Value="2">Both</asp:ListItem>--%>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txt_gen"
                                                PopupControlID="Panel17" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_colord" runat="server" Text="Report Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_colord" runat="server" CssClass="ddlheight3 textbox textbox1">
                                    </asp:DropDownList>
                                </td>
                                <td runat="server" id="tdcbboard" visible="false">
                                    <asp:CheckBox ID="cb_board" Checked="false" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_board_CheckedChanged" />
                                    <asp:Label ID="lbl_board" Text="Board" Style="float: left" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdboard" visible="false">
                                    <asp:TextBox ID="txtBoardUniv" runat="server" CssClass="textbox textbox1 txtheight"
                                        ReadOnly="true" Enabled="false">Board</asp:TextBox>
                                    <asp:Panel ID="pnlBoardUniv" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_BoardUniv" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_BoardUniv_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_BoardUniv" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BoardUniv_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceBoardUniv" runat="server" TargetControlID="txtBoardUniv"
                                        PopupControlID="pnlBoardUniv" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td runat="server" id="tdcbstate" visible="false">
                                    <asp:CheckBox ID="cb_state" Checked="false" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="cb_unistate_CheckedChanged" />
                                    <asp:Label ID="lbl_state" Text="State" Style="float: left" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdstate" visible="false">
                                    <asp:TextBox ID="txtstate" runat="server" CssClass="textbox textbox1 txtheight" ReadOnly="true"
                                        Enabled="false">State</asp:TextBox>
                                    <asp:Panel ID="pnlstate" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_states" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_state_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_state" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_state_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pcestate" runat="server" TargetControlID="txtstate"
                                        PopupControlID="pnlstate" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <%--         Added By Saranyadevi 24.2.2018--%>
                                <td runat="server" id="tdcbdisreason" visible="false">
                                    <asp:CheckBox ID="cb_Disreaason" Checked="false" runat="server" Style="float: left"
                                        AutoPostBack="true" OnCheckedChanged="cb_Disreaason_CheckedChanged" />
                                    <asp:Label ID="lblReason" Text="DisContinue Reason" Style="float: left" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tddisreason" visible="false">
                                    <asp:UpdatePanel ID="UP_reason" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_reason" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true"
                                                Enabled="false" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_reason" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                height: auto;">
                                                <asp:CheckBox ID="cb_reason" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_reason_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_reason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_reason_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_reason" runat="server" TargetControlID="txt_reason"
                                                PopupControlID="panel_reason" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdquota" visible="false">
                                    <asp:CheckBox ID="cbquotacheck" Checked="false" runat="server" Style="float: left"
                                        AutoPostBack="true" OnCheckedChanged="cb_quota_CheckedChanged" />
                                    <asp:Label ID="lblquota" Text="Quota/Category" Style="float: left" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdquotapanel" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtQuota" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true"
                                                Enabled="false" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel18" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                height: auto;">
                                                <asp:CheckBox ID="cbQuota" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbQuota_checkedchange" />
                                                <asp:CheckBoxList ID="cblQuota" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblQuota_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txtQuota"
                                                PopupControlID="panel18" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--abarna--%>
                                <td runat="server" id="tdallotcommunity" visible="false">
                                    <asp:CheckBox ID="allotcommchk" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="allotcommchk_CheckedChanged" />
                                    <asp:Label ID="lblallotcom" Text="Alloted community" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="tdallotcommunity1" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_Allotcomm" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_allotcomm" runat="server" BackColor="White" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_allotcomm" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_allotcomm_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_allotcomm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_allotcomm_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender22" runat="server" TargetControlID="txt_Allotcomm"
                                                PopupControlID="pnl_allotcomm" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="typenametd" visible="false">
                                    <asp:CheckBox ID="chk_typename" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="chk_typename_CheckedChanged" />
                                    <asp:Label ID="lbl_name" Text="Type Name" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="typenametd2" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_Typename" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_name" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_typename" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_typename_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_typename" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_typename_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_Typename"
                                                PopupControlID="pnl_name" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="typesizetd1" visible="false">
                                    <asp:CheckBox ID="chk_typesizename" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="chk_typesizename_CheckedChanged" />
                                    <asp:Label ID="Label2" Text="Type Size" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="typesizetd2" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_typesize" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_size" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_typesize" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_typesize_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_typesize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_typesize_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender24" runat="server" TargetControlID="txt_typesize"
                                                PopupControlID="pnl_size" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="Usernametd" visible="false">
                                    <asp:CheckBox ID="chk_user" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="chk_user_CheckedChanged" />
                                    <asp:Label ID="Label3" Text="User Name" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="UserNameTd2" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Usernametxt" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_user" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_username" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_username_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_username" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_username_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender25" runat="server" TargetControlID="Usernametxt"
                                                PopupControlID="panel_user" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td runat="server" id="refertd" visible="false">
                                    <asp:CheckBox ID="chkrefer" runat="server" Style="float: left" AutoPostBack="true"
                                        OnCheckedChanged="chkrefer_CheckedChanged" />
                                    <asp:Label ID="Label4" Text="Refer By" runat="server"></asp:Label>
                                </td>
                                <td runat="server" id="refertd2" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtReferid" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_refer" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_Refername" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_Refername_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_refername" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_refername_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender26" runat="server" TargetControlID="txtReferid"
                                                PopupControlID="panel_refer" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btndetailgo" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="Go" CssClass="textbox btn1 textbox1" OnClick="btndetailgo_Click" />
                                </td>
                            </tr>
                            <%-- End By Saranyadevi 24.2.2018--%>
                        </table>
                        <br />
                        <div style="border-radius: 7px; width: 400px; margin-left: 722px;">
                            <asp:ImageButton ID="imgbtn_columsetting" Visible="false" runat="server" Width="30px"
                                Height="30px" Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                        </div>
                        <div>
                            <br />
                            <asp:Label ID="lbl_err_stud" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                            <center>
                                <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                                    OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                            <br />
                        </div>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="return ClearPrint()"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcelNew_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="false" OnClick="btnprintmasterNew_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                        </div>
                    </center>
                    <br />
                    <center>
                        <br />
                        <center>
                            <asp:Label ID="lbl_headernamespd2" runat="server" ForeColor="Green" Visible="false"
                                Font-Size="X-Large"></asp:Label>
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Size="X-Large"></asp:Label>
                        </center>
                        <br />
                        <div id="divcolor" runat="server" visible="false">
                            <asp:Label ID="lblappl" runat="server" Text="Applied" Style="color: #b287f2; font-weight: bold;
                                font-family: Book Antiqua;"></asp:Label>
                            <asp:Label ID="lbladmit" runat="server" Text="Admitted" Style="color: #f2c77d; font-weight: bold;
                                font-family: Book Antiqua;"></asp:Label>
                            <asp:Label ID="lbl_discnt" runat="server" Text="DisContinue/Left" Style="color: #F77474;
                                font-weight: bold; font-family: Book Antiqua;"></asp:Label>
                        </div>
                        <br />
                        <%-- <div id="div1" runat="server" style="width: 990px; height: 900; overflow: auto; border: 1px solid Gray;
                                background-color: White;">--%>
                        <asp:UpdatePanel ID="up_spd1" runat="server">
                            <ContentTemplate>
                                <center>
                                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                                        BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                                        OnButtonCommand="fpspread2_ButtonCommand">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- </div>--%>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnk_admisstionformbtn" Font-Names="Book Antiqua" runat="server"
                                        Font-Bold="true" Text="Admission Form" Visible="false" OnClick="lnk_admisstionform_Click" /></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btn_viewsprd2" Font-Names="Book Antiqua" runat="server" Font-Bold="true"
                                        Text="View" Visible="true" OnClick="btn_viewsprd2_Click" /></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:ImageButton ID="img_settingpdf" Visible="false" runat="server" Width="30px"
                                        Height="30px" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="img_settingpdf_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <%-- <asp:UpdatePanel ID="up_popnew" runat="server">
                        <ContentTemplate>--%>
                    <center>
                        <div id="poppernew" runat="server" visible="false" style="height: 355em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="imagebtnpopclose1_Click" />
                            <br />
                            <center>
                                <div class="popsty" style="background-color: White; height: 690px; width: 974px;
                                    border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                                    margin-top: -8px">
                                    <br />
                                    <br />
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                        OnClick="btn_addtype_OnClick" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                                        CssClass="textbox textbox1 ddlheight4">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                        OnClick="btn_deltype_OnClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                    <fieldset style="border-radius: 10px; width: 900px; height: 500px">
                                        <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                                        <table class="table">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lb_selectcolumn" runat="server" SelectionMode="Multiple" Height="490px"
                                                        Width="300px"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <table class="table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnMvOneRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                    Text=">" CssClass="textbox textbox1 btn1" OnClick="btnMvOneRt_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnMvTwoRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                    Text=">>" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoRt_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnMvOneLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                    Text="<" CssClass="textbox textbox1 btn1" OnClick="btnMvOneLt_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnMvTwoLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                    Text="<<" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoLt_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="lb_column1" runat="server" SelectionMode="Multiple" Height="490px"
                                                        Width="300px"></asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                        <br />
                                        <center>
                                            <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                                            <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />
                                        </center>
                                    </fieldset>
                                </div>
                            </center>
                        </div>
                    </center>
                    <%--       </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <asp:UpdatePanel ID="upp_settingpdf" runat="server">
                        <ContentTemplate>
                            <center>
                                <div id="div_settingpdf" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="imgbtn_settingpdf" runat="server" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 7px; margin-left: 468px;"
                                        OnClick="imgbtn_settingpdf_Click" />
                                    <br />
                                    <center>
                                        <div class="popsty" style="background-color: White; height: 650px; width: 957px;
                                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                                            margin-top: 5px">
                                            <br />
                                            <br />
                                            <br />
                                            <fieldset style="border-radius: 10px; width: 868px; height: 550px">
                                                <legend style="font-size: larger; font-weight: bold">PDF Content Settings</legend>
                                                <table class="table">
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="lst_setting1" runat="server" SelectionMode="Multiple" Height="460px"
                                                                Width="300px" ondblclick="ListBox1_DoubleClick()"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <table class="table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneRt1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text=">" CssClass="textbox textbox1 btn1" OnClick="btnMvOneRt1_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoRt1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text=">>" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoRt1_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneLt1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text="<" CssClass="textbox textbox1 btn1" OnClick="btnMvOneLt1_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoLt1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text="<<" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoLt1_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="lst_setting2" runat="server" SelectionMode="Multiple" Height="460px"
                                                                Width="300px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Label ID="lblalerterrnew" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                <br />
                                                <center>
                                                    <asp:Button ID="btnok1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                        Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok1_click" />
                                                    <asp:Button ID="btnclose1" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                        Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose1_click" />
                                                </center>
                                            </fieldset>
                                        </div>
                                    </center>
                                </div>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <div id="div_report" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                            <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                                CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                                Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </div>
                    <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                                height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                    <div id="div_confmcolm" runat="server" visible="false" style="height: 300em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" Text="Are You Want To Set New Report Type"
                                                    Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_confcolm" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_confcolm_Click" Text="ok" runat="server" />
                                                    <asp:Button ID="btn_ntconfcolm" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_ntconfcolm_Click" Text="Cancel" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
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
                </div>
            </div>
        </center>
        <%--    </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <script type="text/javascript">
        function ListBox1_DoubleClick() {
            document.forms[0].lst_setting1.value = "doubleclicked";
            document.forms[0].submit();
        }
    </script>
    </form>
</asp:Content>
