<%@ Page Title="" Language="C#" MasterPageFile="~/smsmod/SMSSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MessageSenderReport.aspx.cs" Inherits="MessageSenderReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function value(val) {
            if (val != "") {
                javascript: window.open("http://www.lbinfotech.biz/PALPAP/");
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">Message Center</span><br />
    </center>
    <center>
        <asp:Panel ID="id11" runat="server" BorderColor="Black" BorderWidth="1px" BackColor="#0CA6CA"
            Style="height: 133px; width: 930px; margin-top: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="25px" Width="400px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbtnstudent" runat="server" Text="Student" AutoPostBack="True"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnCheckedChanged="rdbtnstudent_CheckedChanged" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbtnstaff" runat="server" Text="Staff" Font-Bold="True" AutoPostBack="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rdbtnstaff_CheckedChanged" />
                        <asp:CheckBox ID="chksmsgroup" runat="server" Text=" Group" AutoPostBack="True" OnCheckedChanged="rdbtnsmsGroup_CheckedChanged" />
                        <asp:Button ID="Button2" runat="server" Text="Go" OnClick="btnstaffgo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold; width: 36px;" />
                        <asp:Label ID="lbl_Date" runat="server" Visible="false" Text=" Date" CssClass="commonHeaderFont"
                            Font-Names=" Book antiqua">
                        </asp:Label>
                        <asp:TextBox ID="Txtdate" runat="server" Visible="false" AutoPostBack="true" Style="width: 80px;
                            height: 12px;" CssClass="textbox textbox1"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="Txtdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel ID="studentpanel" runat="server" Width="942px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="tbbat" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                                    Style="height: 20px; width: 80px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="pbat" runat="server" CssClass="multxtpanel" Height="200" Width="125">
                                                    <asp:CheckBox ID="Chkbatsel" runat="server" Text="SelectAll" AutoPostBack="true"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnCheckedChanged="Chkbatsel_CheckedChanged" Checked="true" />
                                                    <asp:CheckBoxList ID="Chkbat" runat="server" Font-Size="Small" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="62px" OnSelectedIndexChanged="Chkbat_SelectedIndexChanged"
                                                        Height="37px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="tbbat"
                                                    PopupControlID="pbat" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Degree" Style="font-family: Book Antiqua; font-size: medium;
                                            font-weight: bold;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="tbdeg" runat="server" ReadOnly="true" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="Pdeg" runat="server" CssClass="multxtpanel" Height="200" Width="125">
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block; color: Black;
                                            font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box"
                                                    Style="font-size: medium; font-weight: bold; height: 20px; width: 100px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="300px" ScrollBars="Auto" CssClass="multxtpanel" Style="font-family: 'Book Antiqua'">
                                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                                    PopupControlID="pbranch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsection" runat="server" Text="Section" Font-Bold="True" Style="display: inline-block;
                                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtsection" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                                    CssClass="Dropdown_Txt_Box" Style="font-size: medium; font-weight: bold; height: 20px;
                                                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="psection" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="125px" ScrollBars="Auto" CssClass="multxtpanel" Style="font-family: 'Book Antiqua'">
                                                    <asp:CheckBox ID="chksection" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                        Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                                    PopupControlID="psection" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblregion" runat="server" Text="Religion" Width="100px" Font-Bold="True"
                                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                            font-weight: bold" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtregion" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                                    CssClass="Dropdown_Txt_Box" Style="font-size: medium; font-weight: bold; height: 20px;
                                                    width: 100px; font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Pregion" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="300px" ScrollBars="Auto" CssClass="multxtpanel" Style="font-family: 'Book Antiqua'">
                                                    <asp:CheckBox ID="chkregion" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkregion_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklsregion" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklsregion_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                        Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtregion"
                                                    PopupControlID="Pregion" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                            font-size: medium; font-weight: bold; width: 36px;" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel ID="staffpanel" runat="server" Width="942px" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department" Style="" Width="90px" Font-Bold="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                                    Style="height: 20px; width: 180px; margin-right: 15px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="400" Width="208">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="SelectAll" AutoPostBack="true"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnCheckedChanged="CheckBox1_CheckedChanged" Checked="true" />
                                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="Small" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="350px" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged"
                                                        Height="37px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBox1"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Staff Type" Style="font-family: Book Antiqua;
                                            font-size: medium; font-weight: bold;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtstafftype" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                                                    ReadOnly="true" Width="180px" Style="height: 20px; width: 100px;" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" BackColor="White" CssClass="multxtpanel" Height="200px"
                                                    ScrollBars="Auto" Width="179px" Style="font-family: 'Book Antiqua'">
                                                    <asp:CheckBox ID="Chkboxstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="Chkboxstafftype_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="Chhliststafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        Width="350px" OnSelectedIndexChanged="Chhliststafftype_SelectedIndexChanged"
                                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Height="58px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtstafftype"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldesignation" runat="server" Text="Designation" Style="display: inline-block;
                                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                            width: 90px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdesignation" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Width="180px" Style="font-size: medium; font-weight: bold; height: 20px;
                                                    width: 180px; font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="pdesignation" runat="server" BackColor="White" CssClass="multxtpanel"
                                                    Height="400px" ScrollBars="Auto" Width="350px" Style="font-family: 'Book Antiqua'">
                                                    <asp:CheckBox ID="chkdesignation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkdesignation_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstdesignation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        Width="350px" OnSelectedIndexChanged="chklstdesignation_SelectedIndexChanged"
                                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Height="58px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtdesignation"
                                                    PopupControlID="pdesignation" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnstaffgo" runat="server" Text="Go" Style="font-family: Book Antiqua;
                                            font-size: medium; font-weight: bold;" OnClick="btnstaffgo_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <fieldset id="fve" runat="server" style="height: 14px; width: 518px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbTrans" runat="server" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblroute" runat="server" Text="Route ID" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
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
                                        <asp:Label ID="Label3" runat="server" Text="Vechile ID" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtvechile" runat="server" Style="height: 20px; width: 86px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_vechile" runat="server" CssClass="multxtpanel" Style="width: 163px;
                                                    height: 300px;">
                                                    <asp:CheckBox ID="cbvechile" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbvechile_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblvechile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblvechile_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtvechile"
                                                    PopupControlID="panel_vechile" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Stage" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtstage" runat="server" Style="height: 20px; width: 76px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_stage" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                    height: 400px;">
                                                    <asp:CheckBox ID="cbstage" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbstage_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblstage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstage_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtstage"
                                                    PopupControlID="panel_stage" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td colspan="3">
                        <fieldset id="fvehicletype" runat="server" style="height: 14px; width: 250px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbvehicleType" runat="server" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Vehicle Type" />
                                        <asp:DropDownList ID="ddlvehType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="25px" Width="120px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <caption>
                    <br />
                    <tr>
                        <td colspan="4">
                            <br />
                            <center>
                                <asp:Panel ID="Panelsmsmail" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px" Style="width: 345px;
                                                    height: 21px;">
                                                    <asp:CheckBox ID="chkboxsms" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                                        font-weight: bold;" Text="SMS" OnCheckedChanged="chkboxsms_CheckedChange" />
                                                    <asp:CheckBox ID="chkboxmail" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                                        font-weight: bold;" Text="MAIL" OnCheckedChanged="chkboxmail_CheckedChange" />
                                                    <asp:CheckBox ID="chknotification" runat="server" AutoPostBack="true" OnCheckedChanged="chknotification_CheckedChange"
                                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                                        font-weight: bold;" Text="Notification" />
                                                    <asp:CheckBox ID="chkvoicecall" runat="server" AutoPostBack="true" OnCheckedChanged="chkvoicecall_CheckedChange"
                                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                                        font-weight: bold;" Text="Voice Call" Visible="false" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="Panel1244" runat="server" BorderColor="Black" BorderWidth="1px" Style="width: 240px;
                                                    height: 21px;">
                                                    <asp:CheckBox ID="chkstudent" runat="server" Font-Names="Book Antiqua" Style="display: inline-block;
                                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                                        Text="Student" />
                                                    <asp:CheckBox ID="chkfather" runat="server" Font-Names="Book Antiqua" Style="display: inline-block;
                                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                                        Text="Father" />
                                                    <asp:CheckBox ID="chkmother" runat="server" Font-Names="Book Antiqua" Style="display: inline-block;
                                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                                        Text="Mother" />
                                                </asp:Panel>
                                            </td>
                                            <%--Added by saranya on 4/9/2018--%>
                                            <td>
                                                <asp:Panel ID="PnlPorAColor" runat="server" Visible="false" BorderColor="Black" BorderWidth="1px"
                                                    Style="width: 165px; height: 21px;">
                                                    <asp:Label ID="LblP" runat="server" Text="" Style="display: inline-block; background-color: Green;
                                                        height: 16px; width: 14px; margin-top: 3px; margin-left: 6px;"></asp:Label>
                                                    <asp:Label ID="LblPresent" runat="server" Text="Present" Font-Bold="True" ForeColor="Black"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    <asp:Label ID="LblAb" runat="server" Text="" Style="display: inline-block; background-color: Red;
                                                        height: 16px; width: 14px; margin-top: 3px; margin-left: 4px;"></asp:Label>
                                                    <asp:Label ID="LblAbsent" runat="server" Text="Absent" Font-Bold="True" ForeColor="Black"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                            <%----------------------------------------%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </center>
                        </td>
                    </tr>
                </caption>
            </table>
        </asp:Panel>
        <div id="Panel2" runat="server" height="18px" style="margin-left: 0px; width: 950px;">
            <asp:Label ID="lblmsgcredit" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
            <asp:Label ID="Label2" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="/"></asp:Label>
            <asp:Label ID="lblmsgused" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
        </div>
        <br />
        <br />
        <asp:Label ID="lblerrsri" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            ForeColor="Red" Font-Size="Medium" Text=""></asp:Label>
        <br />
        <br />
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
        <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
        <asp:Label ID="lblsendmail" runat="server" Width="656px" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Height="25px"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="upgODDetails" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel8">
                    <ProgressTemplate>
                        <center>
                            <div class="CenterPB" style="height: 40px; width: 150px;">
                                <%-- <img alt="" src="../images/progress2.gif" height="180px" width="180px" />--%>
                                <img src="../gv images/cloud_loading_256.gif" style="margin-top: 100px; height: 150px;" />
                                <br />
                                <span style="font-family: Book Antiqua; font-size: medium; color: Gray;">Processing
                                    Please Wait...</span>
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="mPopExtODDetails" runat="server" TargetControlID="upgODDetails"
                    PopupControlID="upgODDetails">
                </asp:ModalPopupExtender>
                <table>
                    <tr>
                        <td>
                            <%--OnButtonCommand="FpSpread1_OnButtonCommand" OnUpdateCommand="FpSpread1_UpdateCommand" --%>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="100px" Width="150px" ActiveSheetViewIndex="0"
                                currentPageIndex="0" OnUpdateCommand="FpSpread1_UpdateCommand" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                                ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonType="PushButton">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                        SelectionForeColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                            <FarPoint:FpSpread ID="fpMsg" runat="server" Visible="false" BorderStyle="Solid"
                                BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="spreadborder" OnButtonCommand="fpMsg_OnButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%--<FarPoint:FpSpread ID="fpMsg" runat="server" Visible="false" Height="100px" Width="150px"
                                OnButtonCommand="fpMsg_OnButtonCommand"  
                                BorderColor="Black" BorderWidth="0.5" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblpurpose1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Purpose"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpurpose" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="300px" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="left">
                            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="250px" Width="1000px" OnCellClick="FpSpread2_CellClick"
                                OnPreRender="FpSpread2_SelectedIndexChanged">
                                <CommandBar BackColor="Control" ButtonType="PushButton">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" SelectionBackColor="#CE5D5A"
                                        SelectionForeColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Height="390px" Width="690px">
                    <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <table>
                            <caption>
                                <br />
                                <br />
                                <br />
                                <caption>
                                    Message Template</caption>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Text="SMS Feed" Font-Size="Large" Font-Names="Book Antiqua"
                                            runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnplus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnplus_Click" Text=" + " />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpurposemsg" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnminus_Click" Text=" - " />
                                    </td>
                                </tr>
                            </caption>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                        Width="680px" Style="font-family: 'Book Antiqua'; margin: 5px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtpurposemsg_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnsave_Click" Height=" 26px" Width=" 88px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexit_Click" />
                                </td>
                            </tr>
                            <caption>
                                <br />
                                <br />
                                <br />
                                <tr>
                                    <td>
                                        <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Style="height: 21px" Width="676px"></asp:Label>
                                    </td>
                                </tr>
                            </caption>
                        </table>
                </asp:Panel>
                <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Height="100px" Width="300px">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                        font-weight: bold; height: 22px; font-family: 'Book Antiqua';"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                        height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        height: 26px;" OnClick="btnpurposeadd_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                                </td>
                            </tr>
                        </table>
                </asp:Panel>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnaddtemplate" runat="server" Text="Add Template" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btndeletetemplate" runat="server" Text="Delete Template" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeletetemplate_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div class="maindivstyle" id="Div7" runat="server" visible="false" style="text-align: left;
                    font-family: MS Sans Serif; font-size: Small; font-weight: bold;">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:RadioButton ID="RbEnglish" runat="server" Checked="true" Text="English" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="RbEnglish_OnCheckedChanged" />
                                    <asp:RadioButton ID="RbTamil" runat="server" Text="Tamil" Font-Bold="True" AutoPostBack="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="RbTamil_OnCheckedChanged"/>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label7" Text="SMS Feed" Font-Size="Large" Font-Names="Book Antiqua"
                                        runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtmessage" runat="server" TextMode="MultiLine" Height="200px" Width="500px"
                                        Style="font-family: 'Book Antiqua'; margin-left: 150px; border-width: thin; border-color: Black;"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <%----   <td>
                            <asp:Button ID="btnsms" runat="server" Text="SEND" Font-Bold="True" Font-Names="Book Antiqua" margin="100px"
                                Font-Size="Medium" OnClick="btnsms_Click" />
                        </td>----%>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnxl_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                </div>
                <br />
                <br />
                <asp:Label ID="errnote" runat="server" Style="" ForeColor="Red" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                <br />
                <div class="maindivstyle" id="Divv1" runat="server" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold" visible="false">
                    <center>
                        <table id="tblmail" runat="server" visible="false">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblmailhead" Text="Mail Feed" Font-Size="Large" Font-Names="Book Antiqua"
                                        runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsub" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px; text-align: center"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsub" runat="server" Style="display: inline-block; color: Black;
                                        border-color: Black; text-align: left; border-width: thin; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbody" Text="Body" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbody" TextMode="MultiLine" runat="server" MaxLength="4000" Style="display: inline-block;
                                        border-color: Black; border-width: thin; color: Black; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px; height: 300px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblattachment" Text="Attachements" runat="server" Font-Bold="true"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;" />
                                    <asp:FileUpload ID="FileUpload2" runat="server" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%----  <td>
                            <asp:Button ID="btnmail" OnClick="btnmail_Click" Text="Send" runat="server" Style="font-family: Book Antiqua;
                                font-size: medium; font-weight: bold;" />
                        </td>-----%>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <br />
                <div class="maindivstyle" id="Divv2" runat="server" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold" visible="false">
                    <center>
                        <table id="Tablenote" runat="server">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblnotification" Text="Notification Feed" Font-Size="Large" Font-Names="Book Antiqua"
                                        runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsubject" runat="server" border-color="black" Style="display: inline-block;
                                        color: Black; border-width: thin; border-color: Black; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnote" Text="Notification" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtnotification" TextMode="MultiLine" runat="server" MaxLength="4000"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        border-width: thin; border-color: Black; font-weight: bold; width: 500px; height: 300px;"></asp:TextBox>
                                </td>
                            </tr>
                            <br></br>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblfile" Text="Photos" runat="server" Font-Bold="true" Style="font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; margin-left: 0px; width: 90px" text-align="left"></asp:Label>
                                    <asp:FileUpload ID="fudfile" runat="server" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblattachements" Text="Attachements" runat="server" Font-Bold="true"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;
                                        text-align: left" />
                                    <asp:FileUpload ID="fudattachemnts" runat="server" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%----  <td>
                            <asp:Button ID="btnnotfsave" OnClick="btnnotfsave_Click" Text="Notification Send"
                                runat="server" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                        </td>---%>
                            </tr>
                        </table>
                    </center>
                </div>
                <%--sms group details showing here--%>
                <asp:Panel ID="Panel4" runat="server" Visible="false" Style="width: auto; height: auto;">
                    <%-- <div id="Div4" style="text-align: center; font-family: MS Sans Serif; font-size: Small;
                        font-weight: bold">--%>
                    <center>
                        <FarPoint:FpSpread ID="FpSpread3" runat="server" Style="width: auto; height: auto;"
                            CssClass="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <%--</div>--%>
                </asp:Panel>
                <%---  <Triggers>
                <asp:PostBackTrigger ControlID="btnsend" />
                <asp:PostBackTrigger ControlID="btnsend"/>
            </Triggers>-----%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <div class="maindivstyle" id="Div5" runat="server" visible="false" style="text-align: center;
            font-family: MS Sans Serif; font-size: Small; font-weight: bold">
            <center>
                <asp:Panel ID="panelvoice" runat="server" Visible="false">
                    <asp:Label ID="Label8" Text="voice Feed" Font-Size="Large" Font-Names="Book Antiqua"
                        runat="server" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lbluploadvoice" runat="server" Text="Upload Voice File" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" margin-left="90px"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnupload" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                        font-weight: bold;" OnClick="btnupload_Click" Text="Save" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_upload" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="blue" Text="Upload Voice file" Visible="false"
                        OnClientClick="value(this);return false"></asp:LinkButton>
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="Fpspreadvoice" runat="server" BorderColor="Black" BorderStyle="Solid"
                        OnUpdateCommand="Fpspreadvoice_Updatecommand" BorderWidth="1px" Height="250px"
                        Width="1000px" ShowHeaderSelection="false">
                        <CommandBar BackColor="Control" ButtonType="PushButton">
                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                        </CommandBar>
                        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" />
                        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" />
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" AutoPostBack="true"
                                SelectionForeColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <%----  <asp:Button ID="btnvoicesave" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                font-weight: bold;" OnClick="btnvoicesave_Click" Text="Send" />----%>
                    &nbsp;&nbsp;
                    <asp:Button ID="btndelete" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                        font-weight: bold;" OnClick="btndelete_Click" Text="Delete" />
                    <asp:Button ID="Button1" runat="server" Text="" Style="opacity: 0;" />
                    <br />
                    <asp:Label ID="lblerrorvoice" Text="" runat="server" Font-Bold="true" ForeColor="Red"
                        Visible="false" font-family="Book Antiqua" Font-Size=" medium" font-weight=" bold" />
                </asp:Panel>
            </center>
        </div>
        <br />
        <br />
        <center>
            <asp:Button ID="btnsend" OnClick="btnsend_Click" Text="Send" runat="server" Style="font-family: Book Antiqua;
                font-size: medium; font-weight: bold;" />
        </center>
        <center>
            <asp:Panel ID="popstud" runat="server" Visible="false" BorderColor="Black" BorderWidth="1px"
                BackColor="#0CA6CA" Style="height: 300px; width: 930px;">
                <div id="popstuds" runat="server" style="height: 100%; z-index: 1000; width: 100%;">
                    <%--background-color: rgba(54, 25, 25, .2);position: absolute; top: 0; left: 0px; display: none;--%>
                    <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 456px;" />
                    <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                    <center>
                        <div id="Div1" runat="server" class="table" style="background-color: White; height: 520px;
                            width: 950px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 10px;
                            border-radius: 10px;">
                            <br />
                            <span style="font-size: larger; color: Green; font-weight: bold;">Student Details</span>
                            <div>
                            </div>
                        </div>
                    </center>
                </div>
            </asp:Panel>
        </center>
    </center>
    <%--Added by saranya 0n 20/9/2018--%>
    <center>
                                    <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                                                    <asp:Button ID="btn_alertclose" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                        font-weight: bold;" OnClick="btn_alertclose_Click" Text="Ok" />
                                                                          
                                                                       
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
