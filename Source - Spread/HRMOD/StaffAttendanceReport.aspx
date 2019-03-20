<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffAttendanceReport.aspx.cs" Inherits="StaffAttendanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width: 952px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div>
            <asp:Panel ID="Pbanner" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 1060px; height: 21px; margin-bottom: 0px; left: -30px; top: 90px;">
                <asp:Label ID="Label2" runat="server" Text="Staff Attendance Report" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" Style="position: absolute;
                    left: 431px;"></asp:Label>
            </asp:Panel>
        </div>
        <br />
        <br />
        <br />
        <div id="maindiv" runat="server" style="background-color: #0CA6CA;">
            <table width="1025">
                <tr>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txt_batch" runat="server" CssClass="Dropdown_Txt_Box" 
Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>--%>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
                        <asp:TextBox ID="txt_dept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pdept" runat="server" CssClass="multxtpanel" Width="250px" Height="300px">
                            <asp:CheckBox ID="chk_dept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_dept_CheckedChanged" />
                            <asp:CheckBoxList ID="chklst_dept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="chklst_dept_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="pextenddept" runat="server" TargetControlID="txt_dept"
                            PopupControlID="pdept" Position="Bottom">
                        </asp:PopupControlExtender>
                        <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
                    </td>
                    <td>
                        <asp:Label ID="lbldesig" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Designation"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                        <asp:TextBox ID="txt_desig" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pdesig" runat="server" CssClass="multxtpanel" Width="250px" Height="300px">
                            <asp:CheckBox ID="chk_desig" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_desig_CheckedChanged" />
                            <asp:CheckBoxList ID="chklst_desig" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="chklst_desig_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="pextenddesig" runat="server" TargetControlID="txt_desig"
                            PopupControlID="pdesig" Position="Bottom">
                        </asp:PopupControlExtender>
                        <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblcategory" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Category"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                        <asp:TextBox ID="txt_category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pcategory" runat="server" CssClass="multxtpanel" Width="200px" Height="150px">
                            <asp:CheckBox ID="chk_category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_category_CheckedChanged" />
                            <asp:CheckBoxList ID="chklst_category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="chklst_category_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="pextendcategory" runat="server" TargetControlID="txt_category"
                            PopupControlID="pcategory" Position="Bottom">
                        </asp:PopupControlExtender>
                        <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblstaff" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Staff Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_staff" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Width="160px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblfromdate" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" Width="90px" Height="20px" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfromdate" runat="server"
                            PopupPosition="BottomRight" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbltodate" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" Width="90px" Height="20px" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttodate" runat="server"
                            PopupPosition="BottomRight" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td colspan="4">
                        <asp:RadioButton ID="rbdaywise" runat="server" Text="Day Wise" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" GroupName="wise" Checked="true" />
                        <asp:RadioButton ID="rbcountwise" runat="server" Text="Count Wise" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="wise" />
                        <asp:CheckBox ID="chklalop" runat="server" Text="Consider Late as LOP" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" />
                    </td>
                    <td>
                        <fieldset id="fldfor" runat="server" style="border-radius: 5px; border-color: Gray;
                            width: 190px; height: 25px;">
                            <asp:RadioButton ID="rdbformat1" runat="server" Checked="true" Text="Format-I" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" AutoPostBack="true" GroupName="rbfor" OnCheckedChanged="rbdformat1_OnCheckedChanged" />
                            <asp:RadioButton ID="rdbformat2" runat="server" Text="Format-II" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" AutoPostBack="true" GroupName="rbfor" OnCheckedChanged="rbdformat2_OnCheckedChanged" />
                        </fieldset>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="GO" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 1085px;
            height: 15px; margin-bottom: 0px; margin-left: -32px;">
        </asp:Panel>
        <asp:Label ID="lblerror" Text="" runat="server" Font-Size="Medium" Font-Bold="True"
            ForeColor="Red" Font-Names="Book Antiqua" />
        <br />
        <asp:Panel ID="pheaderfilter" runat="server" Visible="false" CssClass="cpHeader"
            BackColor="#0CA6CA" Height="14px">
            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
        </asp:Panel>
        <asp:Panel ID="pbodyfilter" runat="server" Visible="false" CssClass="cpBody">
            <table width="810px">
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdbtn_dept_acronym" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                            <asp:ListItem Selected="True">Department Name</asp:ListItem>
                            <asp:ListItem>Department Acronym</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdbtn_desig_acronym" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                            <asp:ListItem Selected="True">Designation Name</asp:ListItem>
                            <asp:ListItem>Designation Acronym</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="cpefilter" runat="server" TargetControlID="pbodyfilter"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
            ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
        <div>
            <asp:Panel ID="PFormat2HeaderFilter" Visible="false" runat="server" CssClass="cpHeader"
                Style="height: 14px; width: 1400px;">
                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                <asp:Label ID="Label1" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" />
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="PFormat2BodyFilter" runat="server" Visible="false" CssClass="cpBody" style="visibility: visible; height: auto;width: 1300px;">
                <table>
                    <tr>
                        <asp:TextBox ID="tborder" Visible="false" Width="1000" TextMode="MultiLine" CssClass="style1"
                            AutoPostBack="true" runat="server">
                        </asp:TextBox>
                        <td>
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Select Column" Width="112px" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblsearch" runat="server" AutoPostBack="true" Height="20px"
                                RepeatColumns="5" RepeatDirection="Horizontal" Style="font-family: 'Book Antiqua';
                                font-weight: 700; font-size: medium;" Width="1100px" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                <asp:ListItem Value="Staff Code">Staff Code</asp:ListItem>
                                <asp:ListItem Value="Staff Name">Staff Name</asp:ListItem>
                                <asp:ListItem Value="Department Name">Department Name</asp:ListItem>
                                <asp:ListItem Value="Department Acronym">Department Acronym</asp:ListItem>
                                <asp:ListItem Value="Designation Name">Designation Name</asp:ListItem>
                                <asp:ListItem Value="Designation Acronym">Designation Acronym</asp:ListItem>
                                <asp:ListItem Value="No.Of Working Days">No.Of Working Days</asp:ListItem>
                                <asp:ListItem Value="Holiday">Holiday</asp:ListItem>
                                <asp:ListItem Value="Total No.Of Days for this Month">Total No.Of Days for this Month</asp:ListItem>
                                <asp:ListItem Value="Total Present Days">Total Present Days</asp:ListItem>
                                <asp:ListItem Value="Total Absent Days">Total Absent Days</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <%--Width="963px"--%>
                    </tr>
                </table>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="PFormat2BodyFilter"
                CollapseControlID="PFormat2HeaderFilter" ExpandControlID="PFormat2HeaderFilter"
                Collapsed="true" TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter"
                CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
        <br />
        <br />
        <FarPoint:FpSpread ID="Fp_StaffAttendance" runat="server" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                <%--<Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>--%>
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                Font-Size="X-Large">
            </TitleInfo>
        </FarPoint:FpSpread>
        <asp:Label ID="lblerrormsg" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
            Style="margin-top: 85px; position: absolute; left: 50px;"></asp:Label>
        <br />
        <table id="lastset" runat="server">
            <tr>
                <td>
                    <asp:Label ID="lblxl" runat="server" Text="Report Name" Style="position: absolute;"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtxl" runat="server" Style="position: absolute; left: 145px;"></asp:TextBox>
                    <asp:Button ID="btnxl" runat="server" Style="position: absolute; left: 310px;" Text="Export Excel"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="position: absolute;
                        left: 443px;" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" />
                    <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
