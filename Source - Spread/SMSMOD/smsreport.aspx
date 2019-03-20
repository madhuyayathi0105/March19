<%@ Page Title="" Language="C#" MasterPageFile="~/smsmod/SMSSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="smsreport.aspx.cs" Inherits="smmreport" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .bakground
        {
            background-color: #666699;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Sms Report</span>
    </center>
    <center>
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="25px" Width="300px">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdbtnstudent" runat="server" Text="Student" AutoPostBack="True"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                        OnCheckedChanged="rdbtnstudent_CheckedChanged" />
                    <asp:RadioButton ID="rdbtnstaff" runat="server" Text="Staff" Font-Bold="True" AutoPostBack="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rdbtnstaff_CheckedChanged" />
                </td>
            </tr>
            <tr runat="server" id="studentpanel">
                <td>
                    <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Batch"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbbat" runat="server" Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="--Select--" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                            <asp:Panel ID="pbat" runat="server" CssClass="multxtpanel" Height="250px">
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
                            <asp:TextBox ID="tbdeg" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                            <asp:Panel ID="Pdeg" runat="server" CssClass="multxtpanel" Height="250px" Width="250px">
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
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                        width: 90px;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-size: medium;
                                font-weight: bold; height: 20px; width: 180px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" Height="250px" CssClass="multxtpanel" Width="250px">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblsection" runat="server" Text="Section" Font-Bold="True" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                        width: 100px;" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtsection" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                Style="font-size: medium; font-weight: bold; height: 20px; width: 100px; font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="psection" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" Height="200px" ScrollBars="Auto" Width="150px" CssClass="multxtpanel"
                                Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chksection" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Width="158px" OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                PopupControlID="psection" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;" />
                </td>
            </tr>
            <tr id="staffpanel" runat="server">
                <td>
                    <asp:Label ID="lbldept" runat="server" Text="Department" Width="90px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="--Select--" Style="height: 20px; width: 180px;">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="400" Width="250">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="SelectAll" AutoPostBack="true"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="CheckBox1_CheckedChanged" Checked="true" />
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="Small" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="350px" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBox1"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Staff Type" Style="font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtstafftype" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                Style="height: 20px; width: 100px;" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px">
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
                                PopupControlID="Panel4" Position="Bottom">
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
                            <asp:TextBox ID="txtdesignation" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                Style="font-size: medium; font-weight: bold; height: 20px; width: 180px; font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdesignation" runat="server" CssClass="multxtpanel" Height="400px">
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
                        font-size: medium; font-weight: bold; height: 33px; width: 36px;" OnClick="btngo_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblst" runat="server" Text="From Date" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtstartdate" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                        font-weight: bold; height: 22px; width: 100px;"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtstartdate" Format="d/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="ft1" runat="server" TargetControlID="txtstartdate"
                        FilterType="Custom,Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td>
                    <asp:Label ID="lblet" runat="server" Text="To Date" Width="60px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                        width: 60px;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtenddate" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                        font-weight: bold; height: 22px;"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="d/MM/yyyy" TargetControlID="txtenddate">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtenddate"
                        FilterType="Custom,Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td colspan="2">
                    <fieldset style="width: 200px; height: 10px;">
                        <asp:RadioButton ID="rbnsms" runat="server" Text="SMS" Font-Bold="True" AutoPostBack="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rbnsms_CheckedChanged"
                            GroupName="sn" />
                        <asp:RadioButton ID="rbnnotification" runat="server" Text="Notification" Font-Bold="True"
                            AutoPostBack="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnCheckedChanged="rbnnotification_CheckedChanged" GroupName="sn" />
                    </fieldset>
                </td>
                <td colspan="2">
                    <fieldset id="fes1" style="width: 150px; height: 10px;">
                        <asp:RadioButton ID="rdnbtndetails" runat="server" Text="Details" Font-Bold="True"
                            AutoPostBack="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnCheckedChanged="rdnbtndetails_CheckedChanged" />
                        <asp:RadioButton ID="rdnbtncount" runat="server" Text="Count" AutoPostBack="True"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnCheckedChanged="rdnbtncount_CheckedChanged" />
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="lblmsgcredit" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
        <%--      <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
            border-right-style: solid; background-color: #0CA6CA; border-width: 1px; width: 950px;" >
            <tr style="height: 35px; padding-bottom: 20px;">
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College Name" Width="150px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
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
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <center>
                        <asp:Panel ID="studentpanel" runat="server" Width="950px">
                            <center>
                                <table style="background-color: #0CA6CA; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Batch"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="tbbat" runat="server" Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="--Select--" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pbat" runat="server" CssClass="multxtpanel" Height="250px">
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
                                                    <asp:TextBox ID="tbdeg" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="height: 20px; width: 100px;">---Select---</asp:TextBox>
                                                    <asp:Panel ID="Pdeg" runat="server" CssClass="multxtpanel" Height="250px" Width="250px">
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
                                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                                                color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                width: 90px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-size: medium;
                                                        font-weight: bold; height: 20px; width: 180px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pbranch" runat="server" Height="250px" CssClass="multxtpanel" Width="250px">
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
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsection" runat="server" Text="Section" Width="100px" Font-Bold="True"
                                                Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                                font-weight: bold; width: 100px;" ForeColor="Black" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtsection" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                                        Style="font-size: medium; font-weight: bold; height: 20px; width: 100px; font-family: 'Book Antiqua';"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                    <asp:Panel ID="psection" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" Height="200px" ScrollBars="Auto" Width="150px" CssClass="multxtpanel"
                                                        Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chksection" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                                            AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                            Width="158px" OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                                        PopupControlID="psection" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                                font-size: medium; font-weight: bold;" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </asp:Panel>
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <center>
                        <asp:Panel ID="staffpanel" runat="server" Width="950px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department" Width="90px" Font-Bold="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="--Select--" Style="height: 20px; width: 180px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="400" Width="208">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="SelectAll" AutoPostBack="true"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnCheckedChanged="CheckBox1_CheckedChanged" Checked="true" />
                                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="Small" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="350px" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged"
                                                        Height="37px">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBox1"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Staff Type" Style="font-family: Book Antiqua;
                                            font-size: medium; font-weight: bold;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtstafftype" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                                    Style="height: 20px; width: 100px;" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px">
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
                                                    PopupControlID="Panel4" Position="Bottom">
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
                                                <asp:TextBox ID="txtdesignation" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                                    Style="font-size: medium; font-weight: bold; height: 20px; width: 180px; font-family: 'Book Antiqua';"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="pdesignation" runat="server" CssClass="multxtpanel" Height="400px">
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
                                            font-size: medium; font-weight: bold; height: 33px; width: 36px;" OnClick="btngo_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblst" runat="server" Text="From Date" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtstartdate" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold; height: 22px; width: 100px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtstartdate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="ft1" runat="server" TargetControlID="txtstartdate"
                                        FilterType="Custom,Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblet" runat="server" Text="To Date" Width="60px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 60px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtenddate" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold; height: 22px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="d/MM/yyyy" TargetControlID="txtenddate">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtenddate"
                                        FilterType="Custom,Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <fieldset style="width: 200px; height: 10px;">
                                        <asp:RadioButton ID="rbnsms" runat="server" Text="SMS" Font-Bold="True" AutoPostBack="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rbnsms_CheckedChanged"
                                            GroupName="sn" />
                                        <asp:RadioButton ID="rbnnotification" runat="server" Text="Notification" Font-Bold="True"
                                            AutoPostBack="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnCheckedChanged="rbnnotification_CheckedChanged" GroupName="sn" />
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset id="fes1" style="width: 150px; height: 10px;">
                                        <asp:RadioButton ID="rdnbtndetails" runat="server" Text="Details" Font-Bold="True"
                                            AutoPostBack="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnCheckedChanged="rdnbtndetails_CheckedChanged" />
                                        <asp:RadioButton ID="rdnbtncount" runat="server" Text="Count" AutoPostBack="True"
                                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnCheckedChanged="rdnbtncount_CheckedChanged" />
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div id="Panel2" runat="server" height="18px" style="margin-left: 0px; width: 950px;">
                        <asp:Label ID="lblmsgcredit" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>--%>
    </center>
    <br />
    <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
    <asp:Label ID="lblsendmail" runat="server" Width="656px" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Height="25px"></asp:Label>
    <div style="">
        <table>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                        currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                        EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                        OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_PreRender">
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
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblfrom" runat="server" Text="Delete From" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtfrom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="50px"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtfrom"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:Label ID="lblto" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="style50"></asp:Label>
                    <asp:TextBox ID="txtto" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="50px"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtto"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnselect" runat="server" Text="Select" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnselect_Click" />
                    <asp:Button ID="btndelete" runat="server" Text="Delete" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btndelete_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
                        Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Visible="False" Style="" CssClass="style50"></asp:Label>
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="panelnotification" runat="server" BorderColor="Black" BackColor="White"
            Visible="false" BorderWidth="2px" Style="" Height="550px" Width="690px">
            <div class="PopupHeaderrstud2" id="Div3" style="text-align: left; font-family: MS Sans Serif;
                font-size: Small; font-weight: bold">
                <br />
                <asp:Label ID="lblnotification" runat="server" Text="Notification" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="XX-Large" Style=""></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblsend" runat="server" Text="Sender :" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style=""></asp:Label>
                <asp:Label ID="lblsender" runat="server" Font-Bold="false" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style=""></asp:Label>
                <asp:Label ID="lblnvdate" runat="server" Text="Date :" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style=""></asp:Label>
                <asp:Label ID="lblndate" runat="server" Font-Bold="false" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style=""></asp:Label>
                <asp:Label ID="lblsubject" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style=""></asp:Label>
                <asp:TextBox ID="txtnotification" runat="server" Enabled="false" ReadOnly="true"
                    TextMode="MultiLine" Style="width: 400px; height: 350px;"></asp:TextBox>
                <asp:Image ID="imgnotification" runat="server" Style="width: 250px; height: 350px;" />
                <asp:Button ID="btnattachement" runat="server" Text="Attachements" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" OnClick="btnattachement_Click" Style="" />
                <asp:Button ID="btnnok" runat="server" Text="Ok" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" OnClick="btnnok_Click" Style="width: 80px;" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
