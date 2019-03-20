<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="stud_hall_rpt.aspx.cs" Inherits="stud_hall_rpt"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
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
        <div class="maindivstyle">
            <center>
                <span class="fontstyleheader" style="font-size: large; color: Green;">Student Hall Report</span>
            </center>
            <table class=" maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="College" Visible="false" Font-Bold="True"
                            Style="font-family: 'Book Antiqua'; color: White;" ForeColor="Black" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                        <asp:DropDownList ID="ddlcollege" Visible="false" runat="server" CssClass="dropdown"
                            Style="font-family: 'Book Antiqua'; margin-left: 10px;" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lbledulevel" runat="server" Text="Edu Level" ForeColor="White" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddledulevel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Width="160" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddledulevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Style="font-family: 'Book Antiqua';
                            color: White;" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        <asp:DropDownList ID="ddltype" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';
                            margin-left: 10px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbach" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';
                            color: White; margin-left: 10px;" Font-Bold="True" ForeColor="Black" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                        <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Size="Medium"
                            Style="width: 84px; font-family: 'Book Antiqua'" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            Font-Bold="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Visible="false" Height="20px" CssClass="dropdown"
                                    ReadOnly="true" Style="width: 84px; font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" Visible="false" BackColor="White" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
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
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="color: White; font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree" runat="server" Height="20px" Visible="true" ReadOnly="true"
                                        CssClass="dropdown" Style="width: 94px; font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" BackColor="White" Visible="true" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
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
                        <asp:Label ID="lblbranch" runat="server" Style="color: White; font-family: 'Book Antiqua';"
                            Text="Branch" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Style="width: 98px; font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
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
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsection" runat="server" Text="Section" Style="color: White; font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsection" runat="server" Height="20px" Visible="true" ReadOnly="true"
                                        CssClass="dropdown" Style="width: 94px; font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="Psection" runat="server" BackColor="White" Visible="true" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" Height="100px" Style="font-family: 'Book Antiqua';
                                        overflow-y: scroll;">
                                        <asp:CheckBox ID="chksection" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksection_CheckedChanged" />
                                        <asp:CheckBoxList ID="chk1section" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chk1section_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtsection"
                                        PopupControlID="Psection" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblterm" runat="server" Font-Color="white" Width="49px" Height="20px"
                            Font-Bold="True" Font-Names="Book Antiqua" Style="float: right; margin-top: -22px;
                            margin-left: 111px;" Font-Size="Medium" Text="Sem" ForeColor="#ffffff"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Visible="true"
                                    OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="width: 64px; font-family: 'Book Antiqua';">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtterm" runat="server" Visible="false" Font-Bold="True" ReadOnly="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="-Select-" Width="80px" Style=""></asp:TextBox>
                                <asp:Panel ID="pnlterm" runat="server" Visible="false" CssClass="MultipleSelectionDDL"
                                    Height="117" Style="" Width="105px">
                                    <asp:CheckBox ID="cbterm" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbterm_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cblterm" runat="server" Font-Size="Small" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblterm_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtterm"
                                    PopupControlID="pnlterm" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblrpttype" runat="server" Text="Report Type" Visible="true" Font-Bold="True"
                            Style="font-family: 'Book Antiqua'; margin-left: 0px; color: White;" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlrpttype" runat="server" Visible="true" CssClass="dropdown"
                            Style="font-family: 'Book Antiqua'; margin-left: 10px;" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlrpttype_SelectedIndexChanged">
                            <asp:ListItem> Student Details </asp:ListItem>
                            <asp:ListItem> Attendance Sheet </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="">
                        <asp:TextBox ID="txtgender" Visible="false" runat="server" Height="20px" CssClass="dropdown"
                            ReadOnly="true" Width="112px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="Panel1" Visible="false" runat="server" BackColor="White" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="2px" Height="115px" Style="font-family: 'Book Antiqua';
                            overflow-y: scroll; width: 156px;">
                            <asp:CheckBox ID="ckgender" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="ckgender_CheckedChanged" />
                            <asp:CheckBoxList ID="cklgender" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Height="58px" OnSelectedIndexChanged="cklgender_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtgender"
                            PopupControlID="Panel1" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblsubjecttype" runat="server" Visible="false" Text="Subject Type"
                            Font-Bold="True" Style="font-family: 'Book Antiqua'; margin-left: 203px; color: White;"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropsubjecttype" Visible="false" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropsubjecttype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfdate" runat="server" Text="From Date" Style="font-family: 'Book Antiqua';
                            color: White;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfdate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfdate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbltodate" runat="server" Text="To Date" Style="font-family: 'Book Antiqua';
                            color: White;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="errmsg" Text="" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cb_includetotal" Text="Include Total" runat="server" />
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblHrDaywise" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                            OnSelectedIndexChanged="rblHrDaywiseClick">
                            <asp:ListItem Selected="True">Day</asp:ListItem>
                            <asp:ListItem>Hour</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Height="30px" Text="Go" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="margin-left: 5px;"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
                ForeColor="#FF3300"></asp:Label>
            <br />
            <center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
            <table id="final" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
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
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print Format 1" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <asp:Button ID="btnprintdirect" runat="server" Text="Print Format 2" OnClick="btnprintdirect_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <asp:Button ID="btnprint" runat="server" Text="Print Format 3" OnClick="btnprint_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <asp:Button ID="btnremarkssave" runat="server" Text="Save" OnClick="btnRemarksave_Click"
                                Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="divPopUpAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopUpAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button2" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopUpAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Label ID="lblxpos" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblypos" runat="server" Visible="false"></asp:Label>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
