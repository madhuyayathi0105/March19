<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="stud_detailedrpt.aspx.cs" Inherits="stud_detailedrpt"
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
    <center>
        <span class="fontstyleheader" style="color: Green;">Students Detailed Report</span>
        <br />
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight5" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
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
                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="font-family: 'Book Antiqua'">
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
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdegree" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Height="300px" Style="font-family: 'Book Antiqua';">
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
            </tr>
            <tr>
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
                    <asp:Label ID="lblrpttype" runat="server" Text="Report Type" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlrpttype" runat="server" CssClass="textbox1 ddlheight3" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlrpttype_SelectedIndexChanged">
                        <asp:ListItem> Ex-Serviceman Student </asp:ListItem>
                        <asp:ListItem> Other University Student  </asp:ListItem>
                        <asp:ListItem> Other State Student </asp:ListItem>
                        <asp:ListItem> International Student </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblstatus" runat="server" Text="Status" Visible="false" Font-Bold="True"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlstatus" runat="server" Visible="false" CssClass="textbox1 ddlheight3"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                        <asp:ListItem>Applied </asp:ListItem>
                        <asp:ListItem>Admitted</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" CssClass="textbox1 btn1" Text="Go" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
                <td>
                    <asp:CheckBox ID="cb_includetotal" Font-Bold="true" runat="server" Text="Include Total" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblerrormsg" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
        <br />
        <center>
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded"
                ShowHeaderSelection="false">
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
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </center>
</asp:Content>
