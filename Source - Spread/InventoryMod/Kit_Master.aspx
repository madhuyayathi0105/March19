<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Kit_Master.aspx.cs" Inherits="InventoryMod_Kit_Master" %>

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
                <span class="fontstyleheader" style="color: Green;">Kit Master</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 976px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true"
                                                        Style="height: auto; width: 231px; margin-left:23px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_itemheadername" runat="server" Text="Item Header Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_headername" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                                <asp:CheckBox ID="cb_headername" runat="server" OnCheckedChanged="cb_headername_CheckedChange"
                                                                    Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cbl_headername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_headername_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_headername"
                                                                PopupControlID="pbatch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Sub Header Name"></asp:Label>
                                                </td>
                                                <td colspan="1">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                                ReadOnly="true" Width="144px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                height: 190px;">
                                                                <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                                    Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_subheadername"
                                                                PopupControlID="Panel2" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                                        OnClick="btn_go_Click" Style="float: right;" BackColor="LightGreen" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_kit" runat="server" Text="Kit"></asp:Label>
                                                   
                                              
                                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                                    <asp:DropDownList ID="ddl_Kitname" OnSelectedIndexChanged="ddl_Kitname_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="ddlheight1 textbox1" runat="server" Style="height: 28px;
                                                        width: 160px;">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
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
        <div id="ShowReport" runat="server" visible="false" style="width: 980px; height: 500px;"
            class="spreadborder">
            <FarPoint:FpSpread ID="spreadDet1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="900px" Height="380px">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <br />
            <asp:Button ID="btn_save" Text="Save" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                OnClick="btn_save_Click" Style="float: none;" BackColor="LightGreen" Width="130px"
                Height="40px" />
            <asp:Button ID="btn_Delete" Text="Delete" CssClass=" textbox btn1" runat="server"
                OnClientClick="return valid2()" OnClick="btn_Delete_Click" Style="float: none;"
                BackColor="LightGreen" Width="130px" Height="40px" />
        </div>
    </center>
    <div>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
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
    <center>
        <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
            <center>
                <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                    height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table style="line-height: 30px">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                    onkeypress="display1()"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="line-height: 35px">
                                <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
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
