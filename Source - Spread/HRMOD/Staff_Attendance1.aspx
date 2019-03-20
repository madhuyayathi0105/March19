<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_Attendance1.aspx.cs" Inherits="Staff_Attendance1" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function reason() {

            document.getElementById('<%=btnaddreason.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnremovereason.ClientID%>').style.display = 'block';
        }
    </script>
    <style type="text/css">
        .font
        {
            font-size: Medium;
            font-family: Book Antiqua;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
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
    <br />
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green">Staff Attendance</span>
        </div>
    </center>
    <table style="background-color: #0CA6CA; border-color: Black; border-top-style: solid;
        border-left-style: solid; border-bottom-style: solid; border-right-style: solid;
        border-width: 1px; width: 100%; height: 70px; top: 130px; position: absolute;
        left: 12px;">
        <tr>
            <td>
                <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Style="left: 5px; top: 6px; position: absolute;" Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="updatecollege" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="139px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="top: 5px; left: 110px; position: absolute;">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lbldep" runat="server" Font-Bold="true" CssClass="font" Text="Department"
                    Style="left: 250px; top: 6px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txt_Department" runat="server" ReadOnly="true" Font-Bold="True"
                            Width="135px" Style="top: 5px; left: 340px; position: absolute;" Font-Names="Book Antiqua"
                            Font-Size="medium" CssClass="Dropdown_Txt_Box">---Select---</asp:TextBox>
                        <asp:Panel ID="panel_Department" runat="server" Height="300px" CssClass="multxtpanel">
                            <asp:CheckBox ID="cb_Department" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Department_CheckedChanged" />
                            <asp:CheckBoxList ID="cbl_Department" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Department_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Department"
                            PopupControlID="panel_Department" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lbldesignation" runat="server" Font-Bold="true" CssClass="font" Text="Designation"
                    Style="left: 481px; top: 6px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel_Designation" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txt_designation" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Style="top: 5px; left: 571px; position: absolute;" Width="135px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="panel_Designation" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                            <asp:CheckBox ID="cb_Designation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Designation_CheckedChanged" />
                            <asp:CheckBoxList ID="cbl_Designation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Designation_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_designation"
                            PopupControlID="panel_Designation" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lblstaffcategory" runat="server" Font-Bold="true" Width="150px" CssClass="font"
                    Style="left: 728px; top: 5px; position: absolute;" Text="Staff Category"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel_Category" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txt_Category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="135px" Style="top: 5px; left: 840px; position: absolute;" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Height="150px">
                            <asp:CheckBox ID="cb_Category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                OnCheckedChanged="cb_Category_CheckedChanged" Font-Size="Medium" Text="Select All"
                                AutoPostBack="True" />
                            <asp:CheckBoxList ID="cbl_Category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                            PopupControlID="panel_Category" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblstafftype" runat="server" Font-Bold="true" CssClass="font" Text="Staff Type"
                    Style="top: 40px; left: 7px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txt_stafftype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="135px" Style="top: 40px; left: 112px; position: absolute;" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="panel_stafftype" runat="server" CssClass="multxtpanel">
                            <asp:CheckBox ID="cbstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                OnCheckedChanged="cbstafftype_CheckedChanged" Font-Size="Medium" Text="Select All"
                                AutoPostBack="True" />
                            <asp:CheckBoxList ID="cblstafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="cblstafftype_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stafftype"
                            PopupControlID="panel_stafftype" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lblattendance" runat="server" Font-Bold="true" CssClass="font" Text="Attendance"
                    Style="top: 40px; left: 252px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtattendance" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="135px" Style="top: 40px; left: 340px; position: absolute;" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pattendance" runat="server" CssClass="multxtpanel" Height="250px">
                            <asp:CheckBox ID="chkattendance" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                OnCheckedChanged="chkattendance_CheckedChanged" Font-Size="Medium" Text="Select All"
                                AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsattendance" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="chklsattendance_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtattendance"
                            PopupControlID="pattendance" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lbldatefrom" runat="server" Font-Bold="true" CssClass="font" Text="From"
                    Style="top: 40px; left: 480px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtfromdate" runat="server" Width="80px" AutoPostBack="True" Style="top: 40px;
                    left: 524px; position: absolute;" Font-Bold="True" OnTextChanged="txtfromdate_TextChanged"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                &nbsp;
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txtfromdate"
                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                <asp:CalendarExtender ID="CalendarExtender22" runat="server" Format="d/MM/yyyy" TargetControlID="txtfromdate">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="lbltodate" runat="server" Font-Bold="true" CssClass="font" Text="To"
                    Style="top: 40px; left: 616px; position: absolute;"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txttodate" runat="server" Width="80px" AutoPostBack="True" Style="top: 40px;
                    left: 642px; position: absolute;" OnTextChanged="txttodate_TextChanged" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txttodate"
                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                <asp:CalendarExtender ID="CalendarExtender23" runat="server" Format="d/MM/yyyy" TargetControlID="txttodate">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:DropDownList ID="ddlsearchappstf" runat="server" Style="top: 38px; position: absolute;
                    left: 733px;" CssClass="textbox1 ddlheight2" OnSelectedIndexChanged="ddlsearchappstf_change"
                    AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txt_staffname" runat="server" MaxLength="100" AutoPostBack="true"
                    OnTextChanged="txt_staffname_change" 
                    Style="width: 135px; top:38px;left:860px;  position: absolute; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                    CompletionListItemCssClass="txtsearchpan">
                </asp:AutoCompleteExtender>
                <asp:TextBox ID="txt_StaffCode" runat="server" MaxLength="100" AutoPostBack="true"
                    OnTextChanged="txt_staffcode_change" 
                    Style="width: 135px; top:38px;left:860px; position: absolute;font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_StaffCode"
                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                    CompletionListItemCssClass="txtsearchpan">
                </asp:AutoCompleteExtender>
            </td>
            <td>
                <asp:Button ID="btn_go" runat="server" Text="Go" Style="top: 38px; position: absolute;
                    left: 1010px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                    CausesValidation="False" OnClick="btn_go_Click" />
            </td>
        </tr>
        <tr>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div>
        <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#0CA6CA"
            Width="936px">
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
    </div>
    <asp:Panel ID="pcolumnorder" runat="server" CssClass="cpBody" Width="800px">
        <table style="background-color: mintcream; width: 936px;">
            <tr>
                <td>
                    <asp:TextBox ID="txt_order" Visible="false" Width="941px" TextMode="MultiLine" CssClass="style1"
                        Style="resize: none;" Height="20px" AutoPostBack="true" runat="server" Enabled="false">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="Cbcolumn" runat="server" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnCheckedChanged="Cbcolumn_CheckedChanged" Style="margin-left: 2.5px;"
                        Text="Select All" />
                    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                    <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                        OnClick="lnk_columnorder_Click" Style="font-family: 'Book Antiqua'; font-weight: 700;
                        font-size: small;" Width="111px">Remove  All</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" AutoPostBack="true" Height="43px"
                        OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged" RepeatColumns="5"
                        RepeatDirection="Horizontal" Style="font-family: 'Book Antiqua'; font-weight: 700;
                        font-size: medium;" Width="928px">
                        <asp:ListItem Selected="True" Value="m.staff_code">Staff Code</asp:ListItem>
                        <asp:ListItem Selected="True" Value="staff_name">Staff Name</asp:ListItem>
                        <asp:ListItem Selected="True" Value="h.dept_name">Department</asp:ListItem>
                        <asp:ListItem Value="d.desig_name">Designation</asp:ListItem>
                        <asp:ListItem Value="s.category_name">Staff Category</asp:ListItem>
                        <asp:ListItem Value="stftype">Staff Type</asp:ListItem>
                        <asp:ListItem Value="Reason">Reason</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
        ExpandedImage="../images/down.jpeg">
    </asp:CollapsiblePanelExtender>
    <br />
    <asp:Label ID="lblError" runat="server" Font-Size="Medium" ForeColor="Red" Font-Bold="True"
        Font-Names="Book Antiqua" Text="" Visible="true"></asp:Label>
    <asp:Label ID="lblerror1" runat="server" Font-Size="Medium" Width="700px" Height="30px"
        ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Text="" Visible="true"></asp:Label>
    <br />
    <asp:UpdatePanel ID="updatejairam" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblerror3" runat="server" Font-Size="Medium" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Text="" Visible="false"></asp:Label>
            <div>
                <asp:Panel ID="panelsecond" runat="server" CssClass="cpHeader" BackColor="#0CA6CA"
                    Visible="false" Style="width: 936px;">
                    <asp:Label ID="lblheaderAttendance" Text="Mark Attendance" runat="server" Font-Size="Medium"
                        Font-Bold="True" Font-Names="Book Antiqua" />
                    <asp:Image ID="imagelabel" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                        ImageAlign="Right" />
                </asp:Panel>
            </div>
            <asp:Panel ID="panelshow" runat="server" CssClass="cpBody" Width="600px">
                <table style="width: 900px;">
                    <tr>
                        <td align="right" style="width: 350px;">
                            <asp:Label ID="lblreasonleave" runat="server" Font-Bold="true" CssClass="font" Text="Leave Reason"
                                Style="left: 638px; position: absolute;" Visible="false"></asp:Label>
                            <asp:Button ID="btnaddreason" runat="server" Text="+" Font-Bold="True" Style="display: none;
                                left: 742px; position: absolute;" OnClick="btnaddreason_Click" Font-Names="Book Antiqua"
                                Font-Size="Small" />
                            <asp:DropDownList ID="ddlleavereason" runat="server" OnSelectedIndexChanged="ddlleavereason_SelectedIndexChanged"
                                Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" Font-Bold="True"
                                Style="left: 776px; position: absolute;" Height="25px" Width="135px">
                            </asp:DropDownList>
                            <asp:Button ID="btnremovereason" runat="server" Text="-" Style="display: none; left: 913px;
                                position: absolute;" Font-Bold="True" OnClick="btnremovereason_Click" Font-Names="Book Antiqua"
                                Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table style="width: 900px;">
                    <tr>
                        <td align="right">
                            <asp:Panel ID="panelreason" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium"
                                Width="150px" Height="100px">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <caption runat="server" id="capreason" title="Leave Reason">
                                                </caption>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_reason" Width="100px" Height="20px" runat="server" Font-Names="Book Antiqua"
                                                    MaxLength="50">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_reason"
                                                    FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnadd1" Width="50px" runat="server" Text="Add" OnClick="btnadd1_Click"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                                &nbsp;
                                                <asp:Button ID="btnexit1" Width="50px" runat="server" Text="Exit" OnClick="btnexit1_Click"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px" OnButtonCommand="FpSpread2_UpdateCommand"
                                ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="MistyRose" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnselectall" runat="server" Text="Select All" Font-Bold="True" OnClick="btnselectall_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" />
                            <asp:Button ID="btndeselectall" runat="server" Text="De-Select All" Font-Bold="True"
                                Visible="false" OnClick="btndeselectall_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" OnClick="btnsave_Click"
                                Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnupdate" runat="server" CssClass="floats" Text="Update" Font-Size="Medium"
                                OnClick="btnupdate_Click" Visible="false" Font-Bold="true" Font-Names="Book Antiqua" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpe2" runat="server" TargetControlID="panelshow"
                CollapseControlID="panelsecond" ExpandControlID="panelsecond" Collapsed="true"
                TextLabelID="lblheaderAttendance" CollapsedSize="0" ImageControlID="imagelabel"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <asp:Panel ID="pnlmsgboxdelete" runat="server" CssClass="modalPopup" Style="display: none;
                height: 100; width: 300;" DefaultButton="btnOk">
                <table width="100%">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="tdCaption" style="background-color: lightblue;">
                            <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnOk" runat="server" Text="Ok" OnClick="btnOk_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfdelete" />
            <asp:ModalPopupExtender ID="mpemsgboxdelete" runat="server" BackgroundCssClass="bakground"
                TargetControlID="hfdelete" PopupControlID="pnlmsgboxdelete">
            </asp:ModalPopupExtender>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnadd1" />
            <asp:PostBackTrigger ControlID="btnexit1" />
            <asp:PostBackTrigger ControlID="btnremovereason" />
            <asp:PostBackTrigger ControlID="btnOk" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="updatejairam">
        <ProgressTemplate>
            <center>
                <div class="CenterPB" style="height: 40px; width: 40px; top: 400px; left: 500px;
                    position: absolute;">
                    <image src="../images/progress2.gif" height="180px" width="180px" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
