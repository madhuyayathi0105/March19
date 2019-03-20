<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Publishresult.aspx.cs" Inherits="Publishresult"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sty
        {
            font-size: medium;
            font-family: Book Antiqua;
            font-weight: bold;
        }
        .multicheckbox
        {
            z-index: 1;
            left: 258px;
            top: -1222px;
            position: absolute;
            overflow: auto;
            background-color: white;
            border: 1px solid gray;
            color: Black;
        }
        .photo
        {
            height: 150px;
            width: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label5" CssClass="fontstyleheader" runat="server" Font-Bold="True"
            ForeColor="Green" Text="Publish Result" Style="margin: 0px; margin-bottom: 15px;
            margin-top: 10px; position: relative;"></asp:Label>
    </center>
    <center>
        <table style="width: auto; height: 78px; background-color: #0CA6CA; margin: 0px;
            margin-bottom: 15px; margin-top: 10px; position: relative; padding: 5px; border-radius: 7px;">
            <tr>
                <td>
                    <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                        ForeColor="Black" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                        ForeColor="Black" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Enabled="true" Font-Size="Medium" Width="90px" CssClass="arrow" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblEduLevel" runat="server" Text="Edu Level" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="upnlEduLevel" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEduLevel" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="85px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pnlEduLevel" runat="server" BackColor="White" BorderColor="Black"
                                    BorderStyle="Solid" CssClass="multicheckbox" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkEduLevel" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkEduLevel_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cblEduLevel" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblEduLevel_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popExtEduLevel" runat="server" TargetControlID="txtEduLevel"
                                    PopupControlID="pnlEduLevel" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="85px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                    Width="85px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                    overflow-y: scroll;">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="85px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                    overflow-y: scroll;">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Button ID="btnsaveresult" runat="server" CssClass="dropdown" Text="Save" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnsaveresult_Click" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="errmsg" runat="server" ForeColor="Red" CssClass="sty" Visible="false"
        Style="margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <div id="showdata" runat="server" style="margin: 0px; margin-bottom: 15px; margin-top: 10px;
            position: relative;">
            <asp:Panel ID="pnlContents" runat="server">
                <div id="printable">
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td colspan="2" align="center">
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            ShowHeaderSelection="false" OnUpdateCommand="FpSpread1_UpdateCommand" BorderWidth="1px"
                                            Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="AsNeeded"
                                            Style="margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
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
                            <tr>
                                <td colspan="2" align="center">
                                    <center>
                                        <asp:Button ID="btnSaveIsFinalYear" runat="server" Height="30px" CssClass="dropdown"
                                            Text="Update" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnSaveIsFinalYear_Click" />
                                        <asp:Button ID="btndeleteresult" runat="server" Height="30px" CssClass="dropdown"
                                            Text="Delete" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeleteresult_Click" />
                                    </center>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </div>
    </center>
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
    </style>
</asp:Content>
