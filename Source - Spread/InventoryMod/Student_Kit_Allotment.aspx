<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Student_Kit_Allotment.aspx.cs" Inherits="InventoryMod_Student_Kit_Allotment" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Student Kit Allotment</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 930px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle" style="width: 900px; height: auto">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                            </asp:DropDownList>--%>
                                                    <%--added by Deepali on 30.3.18--%>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px">
                                                                <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                                                PopupControlID="pbatch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpnlDegree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px">
                                                                <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                                                PopupControlID="pnlDegree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="200px">
                                                                <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                                                PopupControlID="pnlBranch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_section" Text="Section" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_section" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="Panel2" runat="server" unat="server" CssClass="multxtpanel" Height="200px">
                                                                <asp:CheckBox ID="cb_section" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_section_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_section" runat="server" AutoPostBack="true" Font-Bold="True"
                                                                    Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_section_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_section"
                                                                PopupControlID="Panel2" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                                        OnClick="btn_go_Click" Style="float: right;" BackColor="LightGreen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
    </div>
    <br />
    <br />
    <center>
        <div id="ShowReport" runat="server" visible="false" style="width: 738px; height: 410px;"
            class="spreadborder">
            <FarPoint:FpSpread ID="spreadDet1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="726px" Height="330px" OnUpdateCommand="spreadDet1_UpdateCommand">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <div>
                <table>
                    <tr>
                        <td runat="server">
                            <asp:Label ID="lbl_com_ond" Text="Fees Setting:" runat="server" Style=" margin-left:-40px"></asp:Label>
                            <asp:DropDownList ID="ddl_feesetting" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddl_feesetting_SelectedIndexChanged" AutoPostBack="true"
                                Style="height: auto; width: 150px; margin-left:2px; margin-top:2px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_kitename" Text="Kit Name:" runat="server" Style="margin-left:26px; margin-top:7px"></asp:Label>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_kitname" Style="width: 100px; margin-left: 101px; margin-top: -24px;"
                                        runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pan_kit" runat="server" unat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cb_kitname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_kitname_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_kitname" runat="server" AutoPostBack="true" Font-Bold="True"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_kitname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_kitname"
                                        PopupControlID="pan_kit" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                            <td>
                            <asp:DropDownList ID="ddl_Kitname" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddl_Kitname_SelectedIndexChanged" Visible="false" AutoPostBack="true"
                                Style="width: 100px; margin-left: 10px; margin-top: -24px;">
                            </asp:DropDownList>
                       
                            <asp:Button ID="btn_Add_item" Text="Add Kit Item" CssClass=" textbox btn1" runat="server"
                                OnClientClick="return valid2()" OnClick="btn_Add_item_Click" Style="float: none;
                                margin-left: 56px; margin-top: 6px;" BackColor="LightGreen" Width="150px" Height="32px" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--<asp:Button ID="btn_Delete" Text="Delete" CssClass=" textbox btn1" runat="server"
                OnClientClick="return valid2()" OnClick="btn_Delete_Click" Style="float: none;"
                BackColor="LightGreen" Width="130px" Height="40px" />--%>
        </div>
    </center>
    <br />
    <br />
    <center>
        <div id="ShowReport1" runat="server" visible="false" style="width: 680px; height: 400px;"
            class="spreadborder">
            <FarPoint:FpSpread ID="spreadDet2" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="660px" Height="330px">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <%--  style="width:auto 888px; height: 412px;  Width="864px" Height="330px"--%>
            <br />
            <asp:Button ID="btn_kititem_Save" Text="Save" CssClass=" textbox btn1" runat="server"
                OnClientClick="return valid2()" OnClick="btn_kititem_Save_Click" Style="float: none;"
                BackColor="LightGreen" Width="150px" Height="32px" />
        </div>
    </center>
    <center>
        <div id="alertimg" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
</asp:Content>
