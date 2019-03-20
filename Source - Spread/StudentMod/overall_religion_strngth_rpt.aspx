<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="overall_religion_strngth_rpt.aspx.cs" Inherits="overall_religion_strngth_rpt"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
        }
        
        .gvRow td
        {
            background-color: #F0FFFF;
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
        }
        
        .gvAltRow td
        {
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">Overall Religion / Communitywise
            Student Strength Report</span></center>
    <br />
    <center>
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight5" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="280px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbledulevel" runat="server" Text="Edu Level" Width="80px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddledulevel" runat="server" CssClass="textbox1 ddlheight3"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                        OnSelectedIndexChanged="ddledulevel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1 ddlheight3" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbatch" runat="server" CssClass="textbox textbox1 txtheight1"
                                ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Height="200px" Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                PopupControlID="pbatch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Group" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdegree" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Height="200px" Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" CssClass="textbox textbox1 txtheight1"
                                ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="300px" Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblgender" runat="server" Text="Gender" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtgender" runat="server" CssClass="textbox textbox1 txtheight1"
                                Width="126px" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="115px" Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="ckgender" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="ckgender_CheckedChanged" />
                                <asp:CheckBoxList ID="cklgender" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="cklgender_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtgender"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:RadioButton ID="rbreligion" runat="server" Text="Religion" GroupName="vvas"
                        Font-Size="Medium" Font-Bold="True" Width="85px" AutoPostBack="true" OnCheckedChanged="rbreligion_CheckedChanged" />
                </td>
                <td>
                    <asp:RadioButton ID="rbcommunity" runat="server" Text="Community" GroupName="vvas"
                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="rbcommunity_CheckedChanged" />
                </td>
                <td>
                    <asp:Label ID="lblrpttype" runat="server" Text="Religion" Font-Bold="True" Visible="false"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtregcom" runat="server" CssClass="textbox textbox1 txtheight1"
                                ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="115px" Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="chkregcom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkregcom_CheckedChanged" />
                                <asp:CheckBoxList ID="chklregcom" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="chklregcom_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtregcom"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" Height="30px" CssClass="textbox1 btn1" Text="Go"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
    </center>
    <br />
    <br />
    <center>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" CssClass="spreadborder"
            VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <br />
    <center>
        <div id="final" runat="server">
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel" runat="server" CssClass="textbox1 btn2" Font-Bold="true"
                Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel"
                Width="127px" />
            <asp:Button ID="btnprintmaster" runat="server" CssClass="textbox1 btn2" Text="Print"
                OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </center>
</asp:Content>
