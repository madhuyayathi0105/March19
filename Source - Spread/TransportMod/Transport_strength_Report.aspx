<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Transport_strength_Report.aspx.cs" Inherits="Transport_strength_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .modalPopup
        {
            background-color: #696969;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <br />
    <br />
    <br />
     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
    <asp:Panel ID="Panel4" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 70px;
        left: -7px; position: absolute; width: 1030px; height: 21px; margin-bottom: 0px;
        bottom: 300px;">
        <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblhead" runat="server" Text="Transport-Detailed Report" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
        <%-- <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="White" PostBackUrl="~/reports.aspx" CausesValidation="False">Back</asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="White" CausesValidation="False">Logout</asp:LinkButton>--%>
    </asp:Panel>
            </ContentTemplate>
     </asp:UpdatePanel>
     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
    <div style="">
        <table style="margin-left: 10px; margin-top: -180px; position: absolute;">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 176px; left: 10px; height: 22px; width: 80px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_college" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="147px" Style="top: 176px;
                                left: 72px; position: absolute;" ReadOnly="true">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_college" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_college_ChekedChanged" />
                                <asp:CheckBoxList ID="chklst_college" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_college_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_college"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%--<td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_college" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="147px" Style="top: 176px;
                                left: 72px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_college" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_college_ChekedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="chklst_college" runat="server" OnSelectedIndexChanged="chklst_college_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_college"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="lbldistrict" runat="server" Text="District" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 176px; height: 22px; width: 60px; left: 220px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_district" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="144px" Style="top: 176px;
                                left: 280px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_district" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_district_ChekedChanged" />
                                <asp:CheckBoxList ID="chklst_district" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_district_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_district"
                                PopupControlID="Panel6" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_district" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="144px" Style="top: 176px;
                                left: 280px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel6" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_district" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_district_ChekedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="chklst_district" runat="server" OnSelectedIndexChanged="chklst_district_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_district"
                                PopupControlID="Panel6" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="lblstage" runat="server" Text="Stage" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 176px; left: 426px; height: 22px; width: 60px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_stage" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Style="top: 176px;
                                left: 474px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_stage" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_stage_ChekedChanged" />
                                <asp:CheckBoxList ID="chklst_stage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_stage_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stage"
                                PopupControlID="Panel7" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_stage" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Style="top: 176px;
                                left: 474px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel7" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_stage" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_stage_ChekedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="chklst_stage" runat="server" OnSelectedIndexChanged="chklst_stage_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stage"
                                PopupControlID="Panel7" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                 <td>
                    <asp:Label ID="lblvehicle" runat="server" Text="Vehicle" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 176px; left:  805px ; height: 22px; width: 60px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_vehicle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="137px" Style="top: 176px;
                                left: 861px ; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_vehicle" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_vehicle_ChekedChanged" />
                                <asp:CheckBoxList ID="chklst_vehicle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_vehicle_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_vehicle"
                                PopupControlID="Panel8" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_vehicle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="137px" Style="top: 176px;
                                left: 690px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel8" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_vehicle" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_vehicle_ChekedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="chklst_vehicle" runat="server" OnSelectedIndexChanged="chklst_vehicle_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_vehicle"
                                PopupControlID="Panel8" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="lblroute" runat="server" Text="Route" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 176px; height: 22px; width: 60px; left: 632px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_route" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="112px" Style="top: 176px;
                                left: 690px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel9" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_route" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_route_ChekedChanged" />
                                <asp:CheckBoxList ID="chklst_route" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_route_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_route"
                                PopupControlID="Panel9" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_route" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="20px" Width="112px" Style="top: 176px;
                                left: 846px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel9" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_route" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_route_ChekedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="chklst_route" runat="server" OnSelectedIndexChanged="chklst_route_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_route"
                                PopupControlID="Panel9" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
            </tr>
        </table>
        <table style="margin-left: 10px; margin-top: -180px; position: absolute;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 205px; left: 10px; height: 22px; width: 80px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_batch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="90px" Style="top: 205px;
                                left: 70px; height: 20px; width: 80px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_batch" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_batch_CheckedChanged" />
                                <asp:CheckBoxList ID="ddl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_batch"
                                PopupControlID="Panel11" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_batch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="90px" Style="top: 205px;
                                left: 70px; height: 20px; width: 80px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="pnlCustomers" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_batch" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_batch_CheckedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="ddl_batch" runat="server" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_batch"
                                PopupControlID="pnlCustomers" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 205px; left: 152px; height: 22px; width: 120px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_degree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="95px" Style="top: 205px;
                                left: 223px; height: 20px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_degree" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_degree_CheckedChanged" />
                                <asp:CheckBoxList ID="ddl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%--<td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_degree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="95px" Style="top: 205px;
                                left: 223px; height: 20px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_degree" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_degree_CheckedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="ddl_degree" runat="server" OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 205px; left: 318px; height: 22px; width: 70px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_branch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="top: 205px; left: 388px;
                                height: 20px; width: 105px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBox ID="chk_branch" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="chk_branch_CheckedChanged" />
                                <asp:CheckBoxList ID="ddl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_branch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_branch"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_branch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="top: 205px; left: 388px;
                                height: 20px; width: 105px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBox ID="chk_branch" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_branch_CheckedChanged"
                                    Text="Select All" />
                                <asp:CheckBoxList ID="ddl_branch" runat="server" OnSelectedIndexChanged="ddl_branch_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_branch"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Gender" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 205px; left: 493px; height: 22px; width: 60px;
                        position: absolute; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                        background-color: lightblue; border-width: 1px;"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_sex" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="109px" Style="top: 205px;
                                left: 553px; height: 20px; width: 100px; position: absolute;">-- Select--</asp:TextBox>
                            <asp:Panel ID="Panel10" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                <asp:CheckBoxList ID="ddl_sex" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_sex_SelectedIndexChanged">
                                    <asp:ListItem Selected="True"> Male </asp:ListItem>
                                    <asp:ListItem Selected="True"> Female </asp:ListItem>
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_sex"
                                PopupControlID="Panel10" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_sex" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="109px" Style="top: 205px;
                                left: 553px; height: 20px; width: 100px; position: absolute;"></asp:TextBox>
                            <asp:Panel ID="Panel10" runat="server" CssClass="MultipleSelectionDDL" Height="400">
                                <asp:CheckBoxList ID="ddl_sex" runat="server" OnSelectedIndexChanged="ddl_sex_SelectedIndexChanged"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <asp:ListItem Selected="True"> Male </asp:ListItem>
                                    <asp:ListItem Selected="True"> Female </asp:ListItem>
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_sex"
                                PopupControlID="Panel10" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <td>
                    <asp:DropDownList ID="drp_studstaff" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="True" Style="top: 205px; left: 660px; height: 25px; width: 81px; position: absolute;">
                        <asp:ListItem>Student</asp:ListItem>
                        <asp:ListItem>Staff</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:UpdatePanel ID="up3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_student" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="top: 205px; left: 744px; height: 24px; width: 95px;
                                position: absolute;"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <%-- <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress DisplayAfter="500" ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel13">
                                <ProgressTemplate>
                                    <div class="CenterPB" style="height: 40px; width: 40px;">
                                        <img src="images/progress2.gif" height="180px" width="180px" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                                PopupControlID="UpdateProgress1">
                            </asp:ModalPopupExtender>--%>


                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                        <ContentTemplate>
                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="GO_Click" Style="top: 205px; left: 853px; height: 26px;
                        position: absolute;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="Panel5" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="left: 0px;
                        position: absolute; width: 1030px; top: 200px; height: 18px; margin-bottom: 0px;
                        background-image: url('Menu/Top%20Band-2.jpg');">
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Label ID="lblerrmsg" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red"></asp:Label>
        <center>
            <table style="margin-top: 60px; position: absolute;">
                <tr>
                    <td>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel2">
                                    <ProgressTemplate> 
                                        <div class="CenterPB" style="height: 40px; width: 40px;">
                                            <img src="images/progress2.gif" height="180px" width="180px" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
                                    PopupControlID="UpdateProgress2">
                                </asp:ModalPopupExtender>--%>

                        <asp:UpdatePanel ID="spreadUpdatePanel" runat="server">
                            <ContentTemplate>
                        <FarPoint:FpSpread ID="Fp_strength" runat="server" OnCellClick="Fp_strength_CellClick"
                            OnPreRender="Fp_strength_SelectedIndexChanged">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                Font-Size="X-Large">
                            </TitleInfo>
                        </FarPoint:FpSpread>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="False" />
                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        <%--</ContentTemplate>
                              <Triggers>
        <asp:PostBackTrigger ControlID="btnprintmaster" />
    </Triggers>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
            <%--</table>
            <br />
            <table>--%>
                <tr>
                    <td>
                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>--%>
                        <FarPoint:FpSpread ID="Fp_Individual_Strength" runat="server">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                Font-Size="X-Large">
                            </TitleInfo>
                        </FarPoint:FpSpread>
                        <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
            <%--</table>
            <br />
            <table>--%>
                <tr>
                    <td>
                        <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>--%>
                        <FarPoint:FpSpread ID="fp_stud" runat="server" Visible="False">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                Font-Size="X-Large">
                            </TitleInfo>
                        </FarPoint:FpSpread>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                         <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprint_Click" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" Visible="False" />
                    </td>
                </tr>
            </table>
        </center>
        <%-- <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                            <ContentTemplate>--%>
       
        <Insproplus:PRINTPDF runat="server" ID="PRINTPDF1" Visible="false" />
        <%--</ContentTemplate>
            <Triggers>
        <asp:PostBackTrigger ControlID="btnprint" />
    </Triggers>
                        </asp:UpdatePanel>--%>
    </div>
            </ContentTemplate>
     </asp:UpdatePanel>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="spreadUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
