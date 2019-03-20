<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="reportcard_mastersettings.aspx.cs" Inherits="reportcard_mastersettings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div id="Panel1addsub" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0px;
                left: 0px;">
                <center>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Panel ID="Panel1" runat="server" Style="border-color: Black; background-color: #add8e6;
                        z-index: 99; border-style: solid; border-width: 0.5px; width: 235px; height: 150px;">
                        <center>
                            <asp:Label ID="Label2" runat="server" Text="Total No of Subtitle" Font-Bold="true"
                                Font-Size="Medium"></asp:Label></center>
                        <asp:TextBox ID="txttotnosubt" runat="server" Width="200px" MaxLength="1" Style="font-family: 'Book Antiqua';
                            text-align: center;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txttotnosubt"
                            FilterType="numbers" ValidChars="  " />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtactive"
                            FilterType="numbers" ValidChars="  " />
                        <asp:Button ID="btnset" runat="server" Text="Set" Style="height: 28px; width: 80px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                            OnClick="btnset_OnClick" />
                        <asp:Button ID="btnsetexit" runat="server" Text="Exit" Style="height: 28px; width: 80px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnsetexit_OnClick" />
                    </asp:Panel>
                </center>
            </div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="24px"
                Style="width: 100%">
                <center>
                    <asp:Label ID="Label1" runat="server" Text="Report Card - Master Settings" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
                </center>
            </asp:Panel>
            <div style="height: 115px; background-color: LightBlue; border-color: Black; border-style: solid;
                border-width: 1px; width: 1002px; height: auto;">
                <table style="">
                    <tr>
                        <td>
                            <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" ForeColor="Black"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="265px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative">
                                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                                <asp:TextBox ID="tbbat" runat="server" Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="--Select--" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                                <asp:Panel ID="pbat" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                    Style="">
                                    <asp:CheckBox ID="Chkbatsel" runat="server" Text="SelectAll" AutoPostBack="true"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnCheckedChanged="Chkbatsel_CheckedChanged" />
                                    <asp:CheckBoxList ID="Chkbat" runat="server" Font-Size="Small" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="62px" OnSelectedIndexChanged="Chkbat_SelectedIndexChanged"
                                        Height="37px">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="tbbat"
                                    PopupControlID="pbat" Position="Bottom">
                                </asp:PopupControlExtender>
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lbldeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Degree" Style="font-family: Book Antiqua; font-size: medium;
                                font-weight: bold;"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative">
                                <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>--%>
                                <asp:TextBox ID="tbdeg" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                                <asp:Panel ID="Pdeg" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                    Style="">
                                    <asp:CheckBox ID="Chkdegsel" runat="server" Text="SelectAll" AutoPostBack="true"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnCheckedChanged="Chkdegsel_CheckedChanged" />
                                    <asp:CheckBoxList ID="Chkdeg" runat="server" Font-Size="Small" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="98px" OnSelectedIndexChanged="Chkdeg_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="tbdeg"
                                    PopupControlID="Pdeg" Position="Bottom">
                                </asp:PopupControlExtender>
                                <%--  </ContentTemplate>
                            </asp:UpdatePanel>--%>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Black;
                                font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>--%>
                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-size: medium;
                                font-weight: bold; height: 20px; width: 100px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                Style="">
                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Width="180px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Width="350px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
                <table style="">
                    <tr>
                        <td>
                            <span class="fontcomman">Total Parts/Section</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_totparts" Width="58px" MaxLength="1" runat="server" CssClass="fontcomman"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertot5" runat="server" TargetControlID="txt_totparts"
                                FilterType="numbers" ValidChars="  " />
                            <span class="fontcomman">Name</span>
                            <asp:TextBox ID="txt_partname" Width="174px" runat="server" CssClass="fontcomman"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_partname"
                                FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" OnClick="btn_go_OnClick" Width="48px" Text="Go"
                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="fontcomman">Subtitle Master Name</span>
                        </td>
                        <td>
                            <asp:Button ID="titleplus" runat="server" Text="+" Width="30px" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="titleplus_OnClick" />
                            <asp:DropDownList ID="ddltitlename" runat="server" AutoPostBack="true" Width="115px"
                                Font-Names="Book Antiqua" OnSelectedIndexChanged="ddltitlename_OnSelectedIndexChanged"
                                Font-Bold="true" Font-Size="Medium" Height="28px">
                            </asp:DropDownList>
                            <asp:Button ID="titleminus" runat="server" Text="-" Width="30px" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="titleminus_OnClick" />
                            <asp:Button ID="btnacedit" runat="server" Text="Edit" Width="50px" Font-Bold="true"
                                Visible="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnacedit_OnClick" />
                            <span class="fontcomman">Activity Master</span>
                        </td>
                        <td>
                            <asp:Button ID="actplus" runat="server" Text="+" Width="30px" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="actplus_OnClick" />
                            <asp:DropDownList ID="ddlactivity" runat="server" OnSelectedIndexChanged="ddlactivity_OnSelectedIndexChanged"
                                AutoPostBack="true" Width="113px" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="Medium" Height="28px">
                            </asp:DropDownList>
                            <asp:Button ID="actminus" runat="server" Text="-" Width="30px" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="actminus_OnClick" />
                            <asp:Button ID="btnacedit1" runat="server" Text="Edit" Width="50px" Font-Bold="true"
                                Visible="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnacedit1_OnClick" />
                        </td>
                        <td>
                            <span class="fontcomman" style="visibility: hidden;">Description Master</span>
                            <asp:Button ID="btndescplus" runat="server" Text="+" Width="30px" Font-Bold="true"
                                Visible="false" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btndescplus_OnClick" />
                            <asp:DropDownList ID="ddldescrip" runat="server" AutoPostBack="true" Width="113px"
                                Visible="false" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                Height="28px">
                            </asp:DropDownList>
                            <asp:Button ID="btndescminus" runat="server" Text="-" Width="30px" Font-Bold="true"
                                Visible="false" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btndescminus_OnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="fontcomman">Subject Remarks Master</span>
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnrmrkplus" runat="server" Text="+" Width="30px" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnrmrkplus_OnClick" />
                            <asp:DropDownList ID="ddlsubrmrk" runat="server" AutoPostBack="true" Width="115px"
                                Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubrmrk_OnSelectedIndexChanged"
                                Font-Bold="true" Font-Size="Medium" Height="28px">
                            </asp:DropDownList>
                            <asp:Button ID="btnrmrkminus" runat="server" Text="-" Width="30px" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnrmrkminus_OnClick" />
                            <asp:Button ID="btnrmrkedit" runat="server" Text="Edit" Width="50px" Font-Bold="true"
                                Visible="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnrmrkedit_OnClick" />
                            <asp:CheckBox ID="chkPartname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                Font-Bold="true" Text="Need Part Name" />
                        </td>
                    </tr>
                </table>
                <div id="divpnac" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlac" runat="server" Style="border-color: Black; background-color: #add8e6;
                                                    z-index: 99; border-style: solid; border-width: 0.5px; width: 537px; height: 150px;">
                                                    <center>
                                                        <span class="fontcomman">Subtitle Master Name</span>
                                                        <asp:TextBox ID="txtpnlac" Width="250px" runat="server" CssClass="fontcomman"></asp:TextBox>
                                                        <asp:Button ID="btnacsave" runat="server" Text="Save" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                                            OnClick="btnacsave_OnClick" />
                                                        <asp:Button ID="btnacexit" runat="server" Text="Exit" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnacexit_OnClick" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="divpnac1" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlac1" runat="server" Style="border-color: Black; background-color: #add8e6;
                                                    z-index: 99; border-style: solid; border-width: 0.5px; width: 537px; height: 150px;">
                                                    <center>
                                                        <span class="fontcomman">Subtitle Master Name</span>
                                                        <asp:TextBox ID="txtpnlac1" Width="250px" runat="server" CssClass="fontcomman"></asp:TextBox>
                                                        <asp:Button ID="btnacsave1" runat="server" Text="Save" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                                            OnClick="btnacsave1_OnClick" />
                                                        <asp:Button ID="btnacexit1" runat="server" Text="Exit" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnacexit1_OnClick" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="imgdiv1" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnltitle" runat="server" Visible="false" Style="background-color: #add8e6;
                                        border-color: Black; z-index: 99; border-style: solid; border-width: 0.5px; height: 150px;
                                        width: 537px;">
                                        <center>
                                            <asp:Label ID="lbltitname" runat="server" Text="Enter Title Name" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label></center>
                                        <asp:TextBox ID="txttitle" runat="server" Width="505px" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                                        <asp:Button ID="btnadd2" runat="server" Text="Add" Style="height: 28px; width: 80px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                            OnClick="btnadd2_OnClick" />
                                        <asp:Button ID="btnexit2" runat="server" Text="Exit" Style="height: 28px; width: 80px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnexit2_OnClick" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="imgdiv2" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlactive" runat="server" Style="border-color: Black; background-color: #add8e6;
                                                    z-index: 99; border-style: solid; border-width: 0.5px; width: 537px; height: 150px;">
                                                    <center>
                                                        <asp:Label ID="lblactivename" runat="server" Text="Enter Activity Name" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label></center>
                                                    <asp:TextBox ID="txtactive" runat="server" Width="505px" Style="font-family: 'Book Antiqua';"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtactive"
                                                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                                                    <asp:Button ID="btnadd3" runat="server" Text="Add" Style="height: 28px; width: 80px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                                        OnClick="btnadd3_OnClick" />
                                                    <asp:Button ID="btnexit3" runat="server" Text="Exit" Style="height: 28px; width: 80px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnexit3_OnClick" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnldesc" runat="server" Style="border-color: Black; background-color: #add8e6;
                                                    z-index: 99; border-style: solid; border-width: 0.5px; width: 537px; height: 150px;">
                                                    <center>
                                                        <asp:Label ID="Label3" runat="server" Text="Enter Description " Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label></center>
                                                    <asp:TextBox ID="txtdescrip" runat="server" Width="505px" Style="font-family: 'Book Antiqua';"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdescrip"
                                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />--%>
                                                    <asp:Button ID="btndescadd" runat="server" Text="Add" Style="height: 26px; width: 80px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                                        OnClick="btndescadd_OnClick" />
                                                    <asp:Button ID="btndescexit" runat="server" Text="Exit" Style="height: 26px; width: 80px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndescexit_OnClick" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="divSubrmrk" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlsubrmrk" runat="server" Visible="false" Style="background-color: #add8e6;
                                        border-color: Black; z-index: 99; border-style: solid; border-width: 0.5px; height: 150px;
                                        width: 537px;">
                                        <center>
                                            <asp:Label ID="lblsubrmrk" runat="server" Text="Enter Subject Remarks" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label></center>
                                        <asp:TextBox ID="txt_subrmrk" runat="server" Width="505px" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                                        <asp:Button ID="btnaddsubrmrk" runat="server" Text="Add" Style="height: 28px; width: 80px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                            OnClick="btnaddsubrmrk_OnClick" />
                                        <asp:Button ID="btnexit_subrmrk" runat="server" Text="Exit" Style="height: 28px;
                                            width: 80px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnexit_subrmrk_OnClick" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <div id="divpedtsubrmrk" runat="server" visible="false" style="height: 82em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2);">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnledtrmrk" runat="server" Style="border-color: Black; background-color: #add8e6;
                                                    z-index: 99; border-style: solid; border-width: 0.5px; width: 537px; height: 150px;">
                                                    <center>
                                                        <span class="fontcomman">Subject Remarks</span>
                                                        <asp:TextBox ID="txt_edtrmrk" Width="250px" runat="server" CssClass="fontcomman"></asp:TextBox>
                                                        <asp:Button ID="btnedsavermrk" runat="server" Text="Save" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                                            OnClick="btnedsavermrk_OnClick" />
                                                        <asp:Button ID="btnedtexitrmrk" runat="server" Text="Exit" Style="height: 26px; width: 80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnedtexitrmrk_OnClick" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
            <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="21px"
                Style="width: 100%;">
            </asp:Panel>
            <asp:Label ID="lblerrmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblerrmsg1" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblerrmsg2" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                Font-Size="Medium"></asp:Label>
            <center>
                <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                    OnButtonCommand="Fpspread1_Command" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <table>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblFpNewErr" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                                Font-Size="Medium" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnsubti" runat="server" Text="Add Subtitle" Width="120px" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="btnsubti_OnClick" />
                            <asp:Button ID="btnfinalsave" runat="server" Text="Save" Width="80px" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="btnfinalsave_OnClick" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" Width="80px" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="btndelete_OnClick" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </center>
            <style>
                .fontcomman
                {
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                }
            </style>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
