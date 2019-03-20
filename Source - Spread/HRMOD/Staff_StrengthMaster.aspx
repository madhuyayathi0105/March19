<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_StrengthMaster.aspx.cs" Inherits="Staff_StrengthMaster" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/Commonfilter.ascx" TagName="Search" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Staff Strength Master</span>
                </div>
            </center>
        </div>
        <center>
            <fieldset style="width: 975px; height: 265px; margin-left: 0px; border-radius: 10px;
                background-color: #0CA6CA;">
                <table id="tab1" runat="server" style="margin-left: 0px;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_college" runat="server" Text="College Name : " Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="135px"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="updatecollege" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="310px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbldep" runat="server" Text="Department : " Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="p1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_desig" runat="server" Text="Designation :" Style="font-weight: bold;
                                font-family: book antiqua; font-size: medium;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                        <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_desig"
                                        PopupControlID="P2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stype" runat="server" Text="Staff Type :" Style="font-weight: bold;
                                font-family: book antiqua; margin-left: 13px; font-size: medium;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                        <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                        PopupControlID="P4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_scat" runat="server" Text="Staff Category :" Style="font-weight: bold;
                                font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_scat" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_scat" runat="server" Text="Select All" OnCheckedChanged="cb_scat_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_scat" runat="server" OnSelectedIndexChanged="cbl_scat_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_scat"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbblood" runat="server" Text="Blood Group" AutoPostBack="true"
                                OnCheckedChanged="cbblood_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updbgroup" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_bgroup" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlbgroup" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_bgoup" runat="server" Text="Select All" OnCheckedChanged="cb_bgoup_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_bgoup" runat="server" OnSelectedIndexChanged="cbl_bgoup_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_bgroup"
                                        PopupControlID="pnlbgroup" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbmarital" runat="server" Text="Marital Status" AutoPostBack="true"
                                OnCheckedChanged="cbmarital_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updmarital" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_marital" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlmarital" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_marital" runat="server" Text="Select All" OnCheckedChanged="cb_marital_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_marital" runat="server" OnSelectedIndexChanged="cbl_marital_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_marital"
                                        PopupControlID="pnlmarital" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_religieon" runat="server" Text="Religion" AutoPostBack="true"
                                OnCheckedChanged="cb_religieon_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_religieon" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_religieon1" runat="server" Text="Select All" OnCheckedChanged="cb_religieon1_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_religieon1" runat="server" OnSelectedIndexChanged="cbl_religieon1_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_religieon"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cb_comm" runat="server" Text="Community" AutoPostBack="true" OnCheckedChanged="cb_comm_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_comm" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_comm1" runat="server" Text="Select All" OnCheckedChanged="cb_comm1_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_comm1" runat="server" OnSelectedIndexChanged="cbl_comm1_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_comm"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_caste" runat="server" Text="Caste" AutoPostBack="true" OnCheckedChanged="cb_caste_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_caste" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_caste1" runat="server" Text="Select All" OnCheckedChanged="cb_caste1_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_caste1" runat="server" OnSelectedIndexChanged="cbl_caste1_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_caste"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbnation" runat="server" Text="Nationality" AutoPostBack="true"
                                OnCheckedChanged="cbnation_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updnation" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_nation" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlnation" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_nation" runat="server" Text="Select All" OnCheckedChanged="cb_nation_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_nation" runat="server" OnSelectedIndexChanged="cbl_nation_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_nation"
                                        PopupControlID="pnlnation" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cb_fsub" runat="server" Text="Familiar Subjects" AutoPostBack="true"
                                OnCheckedChanged="cb_fsub_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_fsub" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_fsub1" runat="server" Text="Select All" OnCheckedChanged="cb_fsub1_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_fsub1" runat="server" OnSelectedIndexChanged="cbl_fsub1_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_fsub"
                                        PopupControlID="Panel6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_qual" runat="server" Text="Qualification" AutoPostBack="true"
                                OnCheckedChanged="cb_qual_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_qual" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_qual1" runat="server" Text="Select All" OnCheckedChanged="cb_qual1_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_qual1" runat="server" OnSelectedIndexChanged="cbl_qual1_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_qual"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbexp" runat="server" Text="Experience" AutoPostBack="true" OnCheckedChanged="cbexp_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updexp" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_exp" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlexp" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_exp" runat="server" Text="Select All" OnCheckedChanged="cb_exp_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_exp" runat="server" OnSelectedIndexChanged="cbl_exp_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txt_exp"
                                        PopupControlID="pnlexp" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbcity" runat="server" Text="City" AutoPostBack="true" OnCheckedChanged="cbcity_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updcity" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_city" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlcity" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_city" runat="server" Text="Select All" OnCheckedChanged="cb_city_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_city" runat="server" OnSelectedIndexChanged="cbl_city_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txt_city"
                                        PopupControlID="pnlcity" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbdis" runat="server" Text="District" AutoPostBack="true" OnCheckedChanged="cbdis_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updpanel" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dis" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnldis" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_dis" runat="server" Text="Select All" OnCheckedChanged="cb_dis_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_dis" runat="server" OnSelectedIndexChanged="cbl_dis_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_dis"
                                        PopupControlID="pnldis" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbstate" runat="server" Text="State" AutoPostBack="true" OnCheckedChanged="cbstate_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updstate" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_state" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlstate" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_state" runat="server" Text="Select All" OnCheckedChanged="cb_state_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_state" runat="server" OnSelectedIndexChanged="cbl_state_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_state"
                                        PopupControlID="pnlstate" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbgender" runat="server" Text="Gender" AutoPostBack="true" OnCheckedChanged="cbgender_OnCheckedChanged"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updgender" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtgender" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlgender" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="chkgender" runat="server" Text="Select All" OnCheckedChanged="chkgender_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="chklstgender" runat="server" OnSelectedIndexChanged="chklstgender_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txtgender"
                                        PopupControlID="pnlgender" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbstfstatus" runat="server" Text="Staff Status" AutoPostBack="true"
                                OnCheckedChanged="cbstfstatus_OnCheckedChanged" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updstfstatus" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtstfstatus" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; margin-left: 0px; font-family: book antiqua;
                                        font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlstfstatus" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="chkstfstatus" runat="server" Text="Select All" OnCheckedChanged="chkstfstatus_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="chklststfstatus" runat="server" OnSelectedIndexChanged="chklststfstatus_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txtstfstatus"
                                        PopupControlID="pnlstfstatus" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblreportname" runat="server" Text="Report Name" Style="font-weight: bold;
                                font-family: book antiqua; font-size: medium;"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Button ID="btnplusrpt" runat="server" Text="+" CssClass="textbox textbox1 btn2"
                                OnClick="btnplusrpt_click" Width="50px" />
                            <asp:DropDownList ID="ddlrptname" runat="server" CssClass="textbox textbox1 ddlheight5"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlrptname_Change">
                            </asp:DropDownList>
                            <asp:Button ID="btnminusrpt" runat="server" Text="-" CssClass="textbox textbox1 btn2"
                                OnClick="btnminusrpt_click" Width="50px" />
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Button ID="btn_go" Text="Go" runat="server" OnClick="btn_go_Click" Style="font-weight: bold;
                        margin-left: 410px; top: 385px; position: absolute; font-family: book antiqua;
                        font-size: medium; border-radius: 4px;" />
                </center>
                <br />
                <br />
                <asp:ImageButton ID="imgbtn_columsetting" Visible="false" runat="server" Width="30px"
                    Height="30px" Text="All" ImageUrl="../images/images (1)ppp.jpg" Style="margin-left: 900px;"
                    OnClick="imgbtn_all_Click" />
                <center>
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" overflow="true" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Width="970px" Height="500px" class="spreadborder"
                        OnCellClick="Cell_Click" OnPreRender="FpSpread1_Render" Visible="false" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <center>
                    <asp:Label ID="lblspread1_err" runat="server" Text="" Visible="false" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Red"></asp:Label>
                </center>
                <br />
                <center>
                    <FarPoint:FpSpread ID="Fpspread2" runat="server" overflow="true" Visible="false"
                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="970px" Height="500px"
                        OnCellClick="Cell1_Click" OnButtonCommand="FpSpread2_ButtonCommand" class="spreadborder"
                        ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <center>
                    <div id="poppernew" runat="server" visible="false" class="popupstyle popupheight1"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 17px; margin-left: 356px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <div id="Div5" runat="server" class="sty2" style="background-color: White; height: 570px;
                                width: 756px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <fieldset style="border-radius: 10px; width: 680px; height: 500px">
                                    <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lb_selectcolumn" runat="server" SelectionMode="Multiple" Height="380px"
                                                    Width="300px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <table class="table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneRt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoRt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneLt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoLt_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lb_column1" runat="server" SelectionMode="Multiple" Height="380px"
                                                    Width="300px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                    <br />
                                    <center>
                                        <asp:Button ID="btnok" runat="server" Text="OK" CssClass="textbox textbox1 btn2"
                                            OnClick="btnok_click" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="textbox textbox1 btn2"
                                            OnClick="btnclose_click" />
                                    </center>
                                </fieldset>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="div_settingpdf" runat="server" visible="false" class="popupstyle popupheight1"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                        <asp:ImageButton ID="imgbtn_settingpdf" runat="server" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 358px;"
                            OnClick="imgbtn_settingpdf_Click" />
                        <br />
                        <center>
                            <div class="sty2" style="background-color: White; height: 580px; width: 756px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <br />
                                <br />
                                <fieldset style="border-radius: 10px; width: 683px; height: 509px; margin-top: -40px;">
                                    <legend style="font-size: larger; font-weight: bold">PDF Content Settings</legend>
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lst_setting1" runat="server" SelectionMode="Multiple" Height="400px"
                                                    Width="300px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <table class="table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneRt1" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneRt1_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoRt1" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoRt1_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneLt1" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneLt1_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoLt1" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoLt1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lst_setting2" runat="server" SelectionMode="Multiple" Height="400px"
                                                    Width="300px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Label ID="lblalerterrnew" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                    <br />
                                    <center>
                                        <asp:Button ID="btnok1" runat="server" Text="OK" CssClass="textbox textbox1 btn2"
                                            OnClick="btnok1_click" />
                                        <asp:Button ID="btnclose1" runat="server" Text="Close" CssClass="textbox textbox1 btn2"
                                            OnClick="btnclose1_click" />
                                    </center>
                                </fieldset>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 900px; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 400px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: auto; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbladdrptname" runat="server" Text="Report Name" Style="color: Black;"
                                                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtaddrptname" runat="server" CssClass="textbox txtheight1" Width="225px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Button ID="btnsave" CssClass=" textbox1 btn2" OnClick="btnsaverpt_Click" Text="Save"
                                        runat="server" />
                                    <asp:Button ID="btnexit" CssClass=" textbox1 btn2" OnClick="btnexitrpt_Click" Text="Exit"
                                        runat="server" />
                                </center>
                            </div>
                        </center>
                    </div>
                    <div id="popalert" runat="server" visible="false" class="popupstyle popupheight1"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                        <center>
                            <div id="divalert" runat="server" class="table" style="background-color: White; height: auto;
                                width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 220px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: auto; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalrt" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnerrpopclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" Text="Ok" runat="server" OnClick="btnerrpopclose_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <br />
                <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight4"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                        Width="140px" Text="Export To Excel" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2"
                        Width="100px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btn_pdf" runat="server" Text="Generate PDF" OnClick="btn_pdf_OnClick"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2"
                        Width="140px" />
                    <asp:ImageButton ID="img_settingpdf" runat="server" Width="30px" Height="30px" ImageUrl="../images/images (1)ppp.jpg"
                        OnClick="img_settingpdf_Click" />
                </div>
            </fieldset>
        </center>
    </body>
    </html>
</asp:Content>
