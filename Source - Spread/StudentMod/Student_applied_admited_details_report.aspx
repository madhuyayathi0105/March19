<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_applied_admited_details_report.aspx.cs"
    Inherits="StudentMod_Student_applied_admited_details_report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Student Applied
                        and Admitted Student Details </span>
                </div>
            </center>
            <br />
            <div class="maindivstyle maindivstylesize">
                <br />
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
                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 140px; height: 130px;">
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
                            <asp:Label ID="lbl_batch" runat="server" Width="57px" Text="Batch"></asp:Label>
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
                            <asp:Label ID="lbl_status" runat="server" Text="Status"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_status" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true" Enabled="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel9" runat="server" CssClass="multxtpanel" Width="140px" Height="77px"
                                        Style="position: absolute;">
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
                            <asp:CheckBox ID="cb_from" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_from_CheckedChanged" Checked="false" />
                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy">
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
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cb_genderT" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_genderT_CheckedChanged" Text="Gender" />
                        </td>
                        <td runat="server" id="tdgender" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_gender" runat="server" CssClass="textbox textbox1 txtheight"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="77px" Width="100px">
                                        <asp:CheckBox ID="cb_gender" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_gender_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_gender" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_gender_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Male</asp:ListItem>
                                            <asp:ListItem Value="1">Female</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_gender"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_religion1" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_religion_CheckedChanged" Text="Religion" Checked="false" />
                        </td>
                        <td runat="server" id="tdrelichk1" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_religion" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Height="250px" Width="150px">
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
                        <td>
                            <asp:CheckBox ID="cb_com" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_com_CheckedChanged" Text="community" Checked="false" />
                        </td>
                        <td runat="server" id="tdcommchk1" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_comm" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel7" runat="server" CssClass="multxtpanel" Height="250px" Width="140px">
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
                        <td>
                            <asp:CheckBox ID="cb_caste1" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_caste_CheckedChanged" Text="Caste" />
                        </td>
                        <td runat="server" id="tdcaste" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Caste" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Height="160px" Width="140px">
                                        <asp:CheckBox ID="cb_caste" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_caste_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_caste" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_caste_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_Caste"
                                        PopupControlID="Panel10" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cb_boardT" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_boardT_CheckedChanged" Width="140px" Text="Board" Visible="false" />
                            <asp:CheckBox ID="cb_phychallange" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_phychallange_CheckedChanged" Width="145px" Text="PhysicalChallange"
                                Visible="false" />
                        </td>
                        <td id="td_board" runat="server" visible="false">
                            <asp:TextBox ID="txtBoardUniv" runat="server" CssClass="textbox textbox1 txtheight"
                                ReadOnly="true">Board</asp:TextBox>
                            <asp:Panel ID="pnlBoardUniv" runat="server" CssClass="multxtpanel" Width="140px"
                                Height="250px">
                                <asp:CheckBox ID="cb_BoardUniv" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="cb_BoardUniv_checkedchange" />
                                <asp:CheckBoxList ID="cbl_BoardUniv" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BoardUniv_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="pceBoardUniv" runat="server" TargetControlID="txtBoardUniv"
                                PopupControlID="pnlBoardUniv" Position="Bottom">
                            </asp:PopupControlExtender>
                        </td>
                        <td runat="server" id="tdphychallange" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_phychallage" Style="float: left" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel15" runat="server" CssClass="multxtpanel" Height="200px" Width="140px"
                                        Style="position: absolute;">
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
                        <td>
                            <asp:CheckBox ID="cb_maxminum" runat="server" Style="float: left" AutoPostBack="true"
                                OnCheckedChanged="cb_maxminum_CheckedChanged" Width="140px" Text="Mark wise"
                                Visible="false" />
                        </td>
                        <td runat="server" id="td_maxminum" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_maxmin" Style="float: left" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px" Width="140px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_maxmin" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_maxmin_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_maxmin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_maxmin_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_maxmin"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Report Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_reporttype" CssClass="ddlheight4 textbox1" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_reporttype_selectedindexchange">
                                <asp:ListItem Value="0">Religion community caste wise</asp:ListItem>
                                <asp:ListItem Value="1">Board Wise</asp:ListItem>
                                <asp:ListItem Value="2">Disability wise</asp:ListItem>
                                <asp:ListItem Value="3">Communitity wise mark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Text="Go" CssClass="textbox btn1 textbox1" OnClick="btn_go_Click" />
                        </td>
                        <td id="physicalchange_td" runat="server" visible="false">
                            <asp:RadioButton ID="rdbCount" runat="server" Checked="true" Text="Count" GroupName="RepType" />
                            <asp:RadioButton ID="rdbDetail" runat="server" Text="Detail" GroupName="RepType" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                <br />
            </div>
            <br />
            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="245px" Width="950px" Visible="False" VerticalScrollBarPolicy="Never"
                ActiveSheetViewIndex="0">
                <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton">
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                        EditTemplateColumnCount="2" GroupBarText="Drag a column to group by that column."
                        SelectionBackColor="#EAECF5" AutoPostBack="True">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                    VerticalAlign="NotSet" />
            </FarPoint:FpSpread>
            <br />
            <div id="rptprint" runat="server" visible="false">
                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                    Width="180px" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" CssClass="textbox btn1" Text="Export To Excel"
                    Width="127px" OnClick="btnExcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" CssClass="textbox btn1"
                    OnClick="btnprintmaster_Click" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <br />
        </div>
    </center>
    </form>
</asp:Content>
