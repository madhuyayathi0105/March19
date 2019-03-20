<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TransportAllotReport.aspx.cs" Inherits="TransportAllotReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID%>').innerHTML = "";
            }
            function displays() {
                document.getElementById('<%=lblprint.ClientID%>').innerHTML = "";
            }
        
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Transport Allot Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table>
                        <tr>
                            <td>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span style="font-family: Book Antiqua;">Route ID</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP_route" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtroute" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_route" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                        height: 300px;">
                                                        <asp:CheckBox ID="cbroute" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbroute_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblroute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroute_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pce_route" runat="server" TargetControlID="txtroute"
                                                        PopupControlID="panel_route" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <span style="font-family: Book Antiqua;">Vechile ID</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtvechile" runat="server" Style="height: 20px; width: 120px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_vechile" runat="server" CssClass="multxtpanel" Style="width: 163px;
                                                        height: 300px;">
                                                        <asp:CheckBox ID="cbvechile" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbvechile_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblvechile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblvechile_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtvechile"
                                                        PopupControlID="panel_vechile" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <span style="font-family: Book Antiqua;">Stage</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtstage" runat="server" Style="height: 20px; width: 160px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_stage" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: 400px;">
                                                        <asp:CheckBox ID="cbstage" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbstage_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblstage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstage_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtstage"
                                                        PopupControlID="panel_stage" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr id="trstud" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblstr" runat="server" Text="Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                CssClass="textbox  ddlheight" Style="width: 108px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span style="font-family: Book Antiqua;">Batch</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP_batch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                        height: 200px;">
                                                        <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                        PopupControlID="panel_batch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 120px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                        height: 200px;">
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
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 160px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: 300px;">
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
                                    </tr>
                                    <tr>
                                        <%-- <td>
                                        <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtheader" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 126px; height: 120px;">
                                                    <asp:CheckBox ID="cbheader" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbheader_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtheader"
                                                    PopupControlID="pnl_studhed" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtledger" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 126px; height: 120px;">
                                                    <asp:CheckBox ID="cbledger" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbledger_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledger_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtledger"
                                                    PopupControlID="pnl_studled" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                        <%--<td>
                                        <span>Header</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlheader" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" AutoPostBack="true" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span>Ledger</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="118px">
                                        </asp:DropDownList>
                                    </td>--%>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Style="width: 126px; height: 120px;">
                                                        <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                        PopupControlID="pnl_studhed" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Ledger"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Style="width: 126px; height: 120px;">
                                                        <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                        PopupControlID="pnl_studled" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td id="tdlblfin" runat="server" visible="false">
                                            <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                        </td>
                                        <td id="tdfltfin" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtfyear" Style="height: 20px; width: 118px;" CssClass="Dropdown_Txt_Box"
                                                        runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                                        <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                            AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                            AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                                        PopupControlID="Pfyear" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <fieldset style="height: 10px; width: 75px;">
                                                <asp:RadioButton ID="rbstud" runat="server" Text="Student" AutoPostBack="true" OnCheckedChanged="rbstud_Changed"
                                                    GroupName="dt" />
                                                <asp:RadioButton ID="rbstaff" runat="server" Visible="false" Text="Staff" AutoPostBack="true"
                                                    OnCheckedChanged="rbstaff_Changed" GroupName="dt" />
                                                <asp:RadioButton ID="rbboth" runat="server" Visible="false" Text="Both" AutoPostBack="true"
                                                    OnCheckedChanged="rbboth_Changed" GroupName="dt" />
                                            </fieldset>
                                        </td>
                                        <td colspan="2">
                                            <fieldset style="height: 10px; width: 150px;">
                                                <asp:RadioButton ID="rbstage" runat="server" Text="Stage" AutoPostBack="true" OnCheckedChanged="rbstage_Changed"
                                                    GroupName="cl" />
                                                <asp:RadioButton ID="rbroute" runat="server" Text="Route" AutoPostBack="true" OnCheckedChanged="rbroute_Changed"
                                                    GroupName="cl" />
                                            </fieldset>
                                        </td>
                                        <td colspan="4" id="tdstaf" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span style="font-family: Book Antiqua;">Designation</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtdesg" runat="server" Style="height: 20px; width: 154px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_desg" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                    height: 300px;">
                                                                    <asp:CheckBox ID="cbdesg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cbdesg_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbldesg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldesg_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdesg"
                                                                    PopupControlID="panel_desg" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <span style="font-family: Book Antiqua;">Department</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtstafdept" runat="server" Style="height: 20px; width: 164px;"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_stafdept" runat="server" CssClass="multxtpanel" Style="width: 254px;
                                                                    height: 172px;">
                                                                    <asp:CheckBox ID="cbstafdept" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cbstafdept_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblstafdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblstafdept_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtstafdept"
                                                                    PopupControlID="panel_stafdept" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <table>
                                                <tr>
                                                    <td colspan="5">
                                                        <fieldset id="fldmain" runat="server" style="height: 12px; width: 415px;">
                                                            <table id="tblmain" runat="server" style="top: 231px; position: absolute;">
                                                                <tr>
                                                                    <td>
                                                                        <span>Type</span>
                                                                    </td>
                                                                    <td>
                                                                        <fieldset id="fldtype" runat="server" style="height: 10px; width: 233px;">
                                                                            <asp:RadioButton ID="rbterm" runat="server" Text="Term" AutoPostBack="true" OnCheckedChanged="rbterm_Changed"
                                                                                GroupName="li" />
                                                                            <asp:RadioButton ID="rbsem" runat="server" Visible="false" Text="Semester" AutoPostBack="true"
                                                                                OnCheckedChanged="rbsem_Changed" GroupName="li" />
                                                                            <asp:RadioButton ID="rbyear" runat="server" Visible="false" Text="Year" AutoPostBack="true"
                                                                                OnCheckedChanged="rbyear_Changed" GroupName="li" />
                                                                            <asp:RadioButton ID="rball" runat="server" Visible="false" Text="All" AutoPostBack="true"
                                                                                OnCheckedChanged="rball_Changed" GroupName="li" />
                                                                        </fieldset>
                                                                    </td>
                                                                    <td>
                                                                        <fieldset style="height: 10px; width: 100px">
                                                                            <asp:RadioButton ID="rbmonth" runat="server" Text="Month" AutoPostBack="true" OnCheckedChanged="rbmonthChanged"
                                                                                GroupName="li" />
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td colspan="2" id="tdsemyear" runat="server" visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="spsem" runat="server" Text="Semester"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 111px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 143px;
                                                                                height: 219px;">
                                                                                <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sem"
                                                                                PopupControlID="panel_sem" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="2" id="tdmonth" runat="server" visible="false">
                                                        <table id="tablmnth" runat="server" class="tablfont" style="width: 179px; height: 37px;
                                                            border-color: Gray;">
                                                            <tr>
                                                                <td>
                                                                    Month
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtmonth" runat="server" Style="height: 20px; width: 69px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="width: 151px;
                                                                                height: 172px;">
                                                                                <asp:CheckBox ID="cbmonth" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cbmonth_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblmonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblmonth_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtmonth"
                                                                                PopupControlID="panel1" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    Year
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlyear" CssClass="textbox3 textbox1" runat="server" Style="width: 63px;
                                                                        height: 28px;">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Vehicle Type
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtvehtype" runat="server" Style="height: 20px; width: 69px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel2" runat="server" CssClass="multxtpanel" Style="width: 151px;
                                                                    height: 172px;">
                                                                    <asp:CheckBox ID="cbvehtype" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cbvehtype_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblvehtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblvehtype_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtvehtype"
                                                                    PopupControlID="panel2" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="cbdeptacr" runat="server" Text="Department Acronym" />
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="cbcancel" runat="server" Text="Include Cancel" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Width="56px" Text="Go"
                                                            OnClick="btngo_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <div id="gnlcolorder" runat="server" visible="false">
                                    <center>
                                        <div>
                                            <center>
                                                <asp:Panel ID="pnlheading" runat="server" CssClass="cpHeader" Visible="true" Height="22px"
                                                    Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                </asp:Panel>
                                            </center>
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Panel ID="pnlcolorder" runat="server" CssClass="maintablestyle" Width="930px">
                                                <div id="divcolorder" runat="server" style="height: 87px; width: 930px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbcolorder_Changed" />
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                    Visible="false" Width="111px">Remove  All</asp:LinkButton>
                                                                <%--OnClick="lb_Click"--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                    CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="cblcolorder" runat="server" Height="43px" Width="850px" Style="font-family: 'Book Antiqua';
                                                                    font-weight: 700; font-size: medium;" RepeatColumns="5" RepeatDirection="Horizontal">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </center>
                                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlcolorder"
                                        CollapseControlID="pnlheading" ExpandControlID="pnlheading" Collapsed="true"
                                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                        ExpandedImage="~/images/down.jpeg">
                                    </asp:CollapsiblePanelExtender>
                                    <br />
                                    <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                        background-color: White; border-radius: 10px;">
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                                            BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                            class="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchanged">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <br />
                            <td colspan="10">
                                <center>
                                    <div id="print" runat="server" visible="false">
                                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                            CssClass="textbox textbox1" Width="60px" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <%-- <div id="secndcolorder" runat="server" visible="true">
                                <center>
                                    <div>
                                        <center>
                                            <asp:Panel ID="pnlsndcol" runat="server" CssClass="cpHeader" Visible="true" Height="32px"
                                                Width="280px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -718px;">
                                                <asp:Label ID="lblsnd" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                <asp:Button ID="btnsndgo" runat="server" CssClass="textbox btn2" Width="56px" Text="Go"
                                                    OnClick="btnsndgo_Click" Style="margin-top: -0.1%; margin-left: 92px;" />
                                            </asp:Panel>
                                        </center>
                                    </div>
                                    <br />
                                    <div>
                                        <asp:Panel ID="pnlsndcolorder" runat="server" CssClass="maintablestyle" Width="930px">
                                            <div id="div1" runat="server" style="height: 141px; width: 930px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cbsndcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbsndcolorder_Changed" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton3" runat="server" Font-Size="X-Small" Height="16px"
                                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                Visible="false" Width="111px">Remove  All</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtsndcolorder" Visible="true" Width="867px" TextMode="MultiLine"
                                                                CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblsndcolorder" runat="server" Height="43px" Width="850px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="cblsndcolorder_Selected" Style="font-family: 'Book Antiqua';
                                                                font-weight: 700; font-size: medium;" RepeatColumns="5" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </center>
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlsndcolorder"
                                    CollapseControlID="pnlsndcol" ExpandControlID="pnlsndcol" Collapsed="true" TextLabelID="lblsnd"
                                    CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                    ExpandedImage="~/images/down.jpeg">
                                </asp:CollapsiblePanelExtender>
                                <br />
                                <div id="studdet" runat="server" visible="false" style="width: 961px; overflow: auto;
                                    background-color: White; border-radius: 10px;">
                                    <FarPoint:FpSpread ID="fpstud" runat="server" Visible="true" BorderStyle="Solid"
                                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </div>--%>
                                <div id="studdet" runat="server" visible="false" style="width: 961px; overflow: auto;
                                    background-color: White; border-radius: 10px;">
                                    <FarPoint:FpSpread ID="fpstud" runat="server" Visible="true" BorderStyle="Solid"
                                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <div id="subprint" runat="server" visible="false">
                                        <asp:Label ID="lblprint" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtsub" runat="server" Width="180px" onkeypress="displays(this)"
                                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnsubexcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnClick="btnsubexcel_Click" Text="Export To Excel" Width="127px" Height="32px"
                                            CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintm" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Print" OnClick="btnprintm_Click" Height="32px" Style="margin-top: 10px;"
                                            CssClass="textbox textbox1" Width="60px" />
                                        <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
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
    </body>
    </html>
</asp:Content>
