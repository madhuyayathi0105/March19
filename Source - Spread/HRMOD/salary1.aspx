<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="salary1.aspx.cs" Inherits="salary1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/GridPrintMaster.ascx" TagName="GridPrintMaster"
    TagPrefix="InsproplusGrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function fun() {

            if (document.getElementById("cbAuthor").checked = true)
                alert('sss');

        }
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .styles
        {
        }
        #form1
        {
            height: 891px;
            width: 797px;
        }
        .style370
        {
            width: 104px;
        }
        .style405
        {
            width: 36px;
        }
        .style450
        {
            height: 44px;
        }
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
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .style454
        {
            height: 44px;
        }
        .style455
        {
            width: 72px;
        }
        .style458
        {
            width: 122px;
        }
        
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style459
        {
            left: 998px;
            top: 824px;
        }
        .style460
        {
            width: 169px;
            height: 42px;
            left: 897px;
            top: 828px;
        }
        .style461
        {
            height: 33px;
            left: 791px;
            top: 824px;
        }
        .style462
        {
            width: 67px;
            height: 21px;
            left: 719px;
            top: 830px;
        }
        .style463
        {
            width: 40px;
            height: 33px;
            left: 602px;
            top: 824px;
        }
        .style464
        {
            left: 585px;
            top: 830px;
        }
        .style465
        {
            left: 588px;
            top: 843px;
        }
        .style466
        {
            width: 204px;
        }
        .topHandle
        {
            background-color: #97bae6;
        }
        .pop
        {
            background-color: #ffffdd;
            border-width: 1px;
            border-style: solid;
            border-color: Gray;
            min-width: 300px;
            max-width: 350px;
            max-height: 200px;
            min-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
    <body oncontextmenu="return false">
        <script type="text/javascript">
            function pageLoad() {
                var chngPosition = $find('ddeseattype')._dropPopupPopupBehavior;
                chngPosition.set_positioningMode(2);
                var chngPosition1 = $find('ddeblood')._dropPopupPopupBehavior;
                chngPosition1.set_positioningMode(2);
            }
        </script>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Over All Monthly Salary Report</span>
            </div>
        </center>
        <div style="height: 7px; margin-top: 12px;">
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 130px;
                left: 0px; position: absolute; width: 1024px; height: 16px; margin-bottom: 0px;">
            </asp:Panel>
        </div>
        <table style="height: 51px">
            <tr>
                <td>
                    <asp:Label ID="lblmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Month"></asp:Label>
                </td>
                <td class="style405">
                    <asp:DropDownList ID="ddlyear2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="27px" Width="80px">
                    </asp:DropDownList>
                </td>
                <td class="style405">
                    <asp:DropDownList ID="cblmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">March</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                </td>
                <td class="style370">
                    <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Department"></asp:Label>
                    <asp:Label ID="lbldeppt" runat="server" ForeColor="Red" Text="please select any one Department"
                        Visible="False"></asp:Label>
                </td>
                <td class="style458">
                    <asp:TextBox ID="tbseattype" runat="server" Height="24px" ReadOnly="true" Width="150px"
                        OnTextChanged="tbseattype_TextChanged" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <br />
                    <asp:Panel ID="pseattype" runat="server" Height="400px" Width="350px" CssClass="multxtpanel">
                        <asp:CheckBox ID="chkselect" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged1" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="tbseattype"
                        PopupControlID="pseattype" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td>
                    <asp:Label ID="lbldesignation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Designation"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_designation" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                        Width="135px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="panel_Designation" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                        <asp:CheckBox ID="cb_Designation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Designation_CheckedChanged" />
                        <asp:CheckBoxList ID="cbl_Designation" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Designation_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_designation"
                        PopupControlID="panel_Designation" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td class="style455">
                    <asp:Label ID="lblcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Category"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbblood" runat="server" Height="24px" OnTextChanged="tbblood_TextChanged"
                        ReadOnly="true" Width="135px" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True">---Select---</asp:TextBox>
                    <asp:Panel ID="pblood" runat="server" Width="200px" CssClass="multxtpanel">
                        <asp:CheckBox ID="chkcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="tbblood"
                        PopupControlID="pblood" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Date" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="20px"
                        Width="75px" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="True" OnTextChanged="Txtentryfrom_TextChanged"
                        Font-Size="Medium" Visible="False"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txtentryfrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="To" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txtentryto" runat="server" Height="20px" Width="75px" Font-Bold="True"
                        Font-Names="Book Antiqua" OnTextChanged="Txtentryto_TextChanged" Font-Size="Medium"
                        Visible="False"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="13">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Staff Type"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_stafftype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="135px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panel_stafftype" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="cbstafftype_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cblstafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="cblstafftype_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stafftype"
                                    PopupControlID="panel_stafftype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="PF Category" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlpfcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Value="1">PF Staff</asp:ListItem>
                                    <asp:ListItem Value="0">Non PF Staff</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_collBank" runat="server" Text="College Bank" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_colbank" runat="server" Width="150px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkteachnonteach" runat="server" Text="Staff Name Settings" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnkteachnonteach_click"></asp:LinkButton>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <%--delsi0404--%>
                            <td>
                                <asp:LinkButton ID="lnk_btn_print" runat="server" Text="Print Settings" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnk_btn_print_click"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbAllowdeduc" runat="server" Visible="false" Text="Show Description" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div>
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#0CA6CA"
                Height="14px">
                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="style454">
                            <asp:CheckBoxList ID="cblsearch" runat="server" Height="43px" Width="850px" AutoPostBack="true"
                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="cblsearch_SelectedIndexChanged">
                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                <asp:ListItem Value="1">Designation</asp:ListItem>
                                <asp:ListItem Value="2">Department</asp:ListItem>
                                <asp:ListItem Value="3">Dept acronym</asp:ListItem>
                                <asp:ListItem Value="4">Desig Acronym</asp:ListItem>
                                <asp:ListItem Value="5">Category</asp:ListItem>
                                <asp:ListItem Value="6">LOP Days </asp:ListItem>
                                <asp:ListItem Value="7">LOP  Dates</asp:ListItem>
                                <asp:ListItem Value="8">Date Of Joining</asp:ListItem>
                                <asp:ListItem Value="9">Account No</asp:ListItem>
                                <asp:ListItem Value="10">Actual basic</asp:ListItem>
                                <asp:ListItem Value="11">LOP Amount</asp:ListItem>
                                <asp:ListItem Value="12">Pf Number</asp:ListItem>
                                <asp:ListItem Value="13">DA %</asp:ListItem>
                                <asp:ListItem Value="14">Select</asp:ListItem>
                                <asp:ListItem Value="15">Staff Type</asp:ListItem>
                                <asp:ListItem Value="16">Basic</asp:ListItem>
                                <asp:ListItem Value="17">AGP</asp:ListItem>
                                <asp:ListItem Value="18">No. of Increment</asp:ListItem>
                                <asp:ListItem Value="19">Increment in Rs.</asp:ListItem>
                                <asp:ListItem Value="20">Working Days</asp:ListItem>
                                <asp:ListItem Value="21">Basic Pay</asp:ListItem>
                                <asp:ListItem Value="22">Actual Gross Pay</asp:ListItem>
                                <asp:ListItem Value="23">Gross Pay</asp:ListItem>
                                <asp:ListItem Value="24">Adv Rs.</asp:ListItem>
                                <asp:ListItem Value="25">Collected amt</asp:ListItem>
                                <asp:ListItem Value="26">Check no</asp:ListItem>
                                <asp:ListItem Value="27">Signature</asp:ListItem>
                                <asp:ListItem Value="28">Total Deduction</asp:ListItem>
                                <asp:ListItem Value="29">Absent Days</asp:ListItem>
                                <asp:ListItem Value="30">Staff Code</asp:ListItem>
                                <asp:ListItem Value="31">Pay Band</asp:ListItem>
                                <asp:ListItem Value="32">Grade Pay</asp:ListItem>
                                <asp:ListItem Value="33"> EPF A/C No</asp:ListItem>
                                <asp:ListItem Value="34"> Pay Scale</asp:ListItem>
                                <asp:ListItem Value="35">Department With Pay Scale</asp:ListItem>
                                <asp:ListItem Value="36">Title</asp:ListItem>
                                <asp:ListItem Value="37">Provident Fund Earning</asp:ListItem>
                                <asp:ListItem Value="38">Present Days</asp:ListItem>
                                <asp:ListItem Value="39">With Revenue Stamp</asp:ListItem>
                                <asp:ListItem Value="40">Bank Formate</asp:ListItem>
                                <asp:ListItem Value="41">Office Seal</asp:ListItem>
                                <asp:ListItem Value="42">Bank Name</asp:ListItem>
                                <%--delsi25/05--%>
                                <asp:ListItem Value="43">College Bank</asp:ListItem>
                                <asp:ListItem Value="44">UAN No</asp:ListItem>
                                <asp:ListItem Value="45">Gratuity</asp:ListItem>
                                <asp:ListItem Value="46">YEAR OF EXPERIENCE</asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:FileUpload ID="fileuploadbrowse" runat="server" />
                            <asp:Button ID="btnupload" runat="server" Text="Upload" OnClick="btnupload_Click" />
                            <asp:Label ID="lblIndicate" runat="server" Text="(.jpg/.jpeg)" ForeColor="Red" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style454">
                            &nbsp;
                        </td>
                        <td class="style454">
                        </td>
                        <td class="style454">
                        </td>
                        <td class="style454">
                            <asp:LinkButton ID="LinkButtonsremove" runat="server" Font-Size="X-Small" Height="16px"
                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: 0px;"
                                Visible="false" Width="111px">Remove  All</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpefilter" runat="server" TargetControlID="pbodyfilter"
                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Allowance"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtallowance" runat="server" Height="20px" ReadOnly="true" Width="135px"
                        OnTextChanged="tbseattype_TextChanged" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="Pallowance" runat="server" CssClass="multxtpanel">
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="CheckBox1_CheckedChanged1"
                            Text="Select All" Checked="True" />
                        <asp:CheckBoxList ID="cblallowance" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="cblallowance_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtallowance"
                        PopupControlID="Pallowance" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td>
                    <asp:Label ID="lblde" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Deduction"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdeduction" runat="server" Height="20px" ReadOnly="true" Style="font-family: 'Book Antiqua';
                        margin-bottom: 0px;" CssClass="Dropdown_Txt_Box" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="Pdeduction" runat="server" CssClass="multxtpanel" Width="200px">
                        <asp:CheckBox ID="chkdeduc" runat="server" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkdeduc_CheckedChanged" Text="Select All"
                            Checked="True" />
                        <asp:CheckBoxList ID="cbldeduction" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbldeduction_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeduction"
                        PopupControlID="Pdeduction" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td>
                    <asp:Label ID="lblorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Order by"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlorder" runat="server" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                        <asp:ListItem Text="Dept & Staff Code"></asp:ListItem>
                        <asp:ListItem Text="Priority"></asp:ListItem>
                        <asp:ListItem Text="Print Priority-1"></asp:ListItem>
                        <asp:ListItem Text="Print Priority-2"></asp:ListItem>
                        <asp:ListItem Text="Account No"></asp:ListItem>
                        <asp:ListItem Text="Staff Wise Priority"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="chksms" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Visible="false" Font-Size="Medium" Text="SMS" AutoPostBack="true" />
                </td>
                <td>
                    <asp:CheckBox ID="chkEmail" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Visible="false" Font-Size="Medium" Text="E-Mail" AutoPostBack="true" />
                </td>
                <td>
                    <asp:Button ID="btn_salarynew" runat="server" Visible="false" Text="Salary Certificate Formate 2"
                        OnClick="btnsalary_Click" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblreport" runat="server" Text="Report" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlreport" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged">
                        <asp:ListItem Text="Format 1"></asp:ListItem>
                        <asp:ListItem Text="Format 2"></asp:ListItem>
                        <asp:ListItem Text="Format 3"></asp:ListItem>
                        <asp:ListItem Text="Format 4"></asp:ListItem>
                        <asp:ListItem Text="Department Wise"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="rddept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Dept Wise" />
                </td>
                <td>
                    <asp:CheckBox ID="chkactual" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Actual" Visible="False" Width="75px" />
                </td>
                <td>
                    <asp:CheckBox ID="chkcontribution" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Contributions" />
                </td>
                <td>
                    <asp:Label ID="lblstaffcnt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="No Of Staff Per Page" Visible="False" Width="159px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_staff" runat="server" Font-Bold="True" Font-Size="Medium" Width="33px"
                        OnTextChanged="txt_staff_TextChanged" Visible="False">6</asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btngo_Click" Text="GO" CssClass="style463" />
                </td>
                <td>
                    <asp:CheckBox ID="cbkincludeCarryTot" runat="server" Text="Include Carry over" Font-Bold="True"
                        Visible="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td>
                    <asp:CheckBox ID="chk_bf" runat="server" Text="Include Borrow From" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td>
                    <asp:CheckBox ID="cb_hold" runat="server" Text="Include Salary Hold Staff" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" />
                </td>
                <td>
                    <asp:CheckBox ID="chkdays" runat="server" Text="Attendance Details" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td>
                    <asp:Button ID="btnslip" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnslip_Click" Text="Salary Slip" Width="106px" Visible="False"
                        CssClass="style461" />
                    <asp:Label ID="lblselect" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="Please Select The Staff" Visible="False"
                        CssClass="style460"></asp:Label>
                    <asp:Button ID="tn_format2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="tn_format2_Click" Text="Salary Slip" Visible="False" />
                </td>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="Print" Width="57px" Height="33px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="False" CssClass="style459"
                        OnClick="Button2_Click" />
                </td>
                <td>
                    <asp:Button ID="salcer" runat="server" Text="Salary Certificate" Width="138px" Height="33px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
                        CssClass="style459" OnClick="salcer_Click" />
                </td>
                <td>
                    <asp:Button ID="btnpayslip" runat="server" Text="Pay Slip" Height="33px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style459" OnClick="btnpayslip_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="cbdateofjoin" runat="server" Text="Date of Join" Visible="false" />
                    <asp:CheckBox ID="cbRound" runat="server" Text="Round Off" Visible="false" />
                </td>
                <td colspan="4">
                    <asp:CheckBox ID="cbrupesinwords" runat="server" Text="Net Amount in Words" Font-Bold="True"
                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td colspan="9">
                    <asp:CheckBox ID="cbAuthor" runat="server" Font-Bold="True" Visible="false" AutoPostBack="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbAuthor_CheckedChanged" />
                    <asp:Label ID="lblauthor" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Authorized" Visible="false"></asp:Label>
                    <asp:TextBox ID="txt_author" runat="server" Font-Bold="True" Enabled="false" Width="150px"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_author"
                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars="  ">
                    </asp:FilteredTextBoxExtender>
                    <asp:CheckBox ID="cbincludeBasicpay" runat="server" Text="Not Include Basic Pay"
                        Font-Bold="True" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
        <table>
        </table>
        <asp:Panel ID="Panel8" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Width="1024px" Style="position: absolute; left: 0px;">
        </asp:Panel>
        <br />
        <br />
        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            ForeColor="Red" Text="No Record Were Found" Visible="False"></asp:Label>
        <br />
        <br />
        <FarPoint:FpSpread ID="fpsalary" runat="server" BorderColor="Black" BorderStyle="Solid"
            OnButtonCommand="fpsalary_UpdateCommand" BorderWidth="1px" Height="245px" Width="950px"
            Visible="False" VerticalScrollBarPolicy="Never" ActiveSheetViewIndex="0" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton">
                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                    EditTemplateColumnCount="2" GridLineColor="#D0D7E5" GroupBarText="Drag a column to group by that column."
                    SelectionBackColor="#EAECF5">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                VerticalAlign="NotSet" />
        </FarPoint:FpSpread>
        <br />
        <asp:Label ID="lblsmserror" Text="" Font-Size="Large" Font-Names="Book Antiqua" Visible="false"
            ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="False"></asp:Label>
        <asp:TextBox ID="txtexcel" onkeypress="display()" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" runat="server"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnexcel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Export Excel" OnClick="btnexcel_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print New" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <asp:Button ID="Button3" runat="server" Text="Print Old" OnClick="btnButton3_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <InsproplusGrid:GridPrintMaster runat="server" ID="GridPrintmaster" Visible="false" />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblsmstype" Text="SMS Type" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsmstype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlsmstype_SelectedIndexChanged">
                        <asp:ListItem Text="Automatic"></asp:ListItem>
                        <asp:ListItem Text="Template With Salary"></asp:ListItem>
                        <asp:ListItem Text="Template With Out Salary"></asp:ListItem>
                        <asp:ListItem Text="Template With LOP"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
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
            <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
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
                                Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnTextChanged="txtpurposemsg_TextChanged"></asp:TextBox>
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
                                    Font-Size="Medium" ForeColor="Red" Style="top: 340px; left: 5px; position: absolute;
                                    height: 21px" Width="676px"></asp:Label>
                            </td>
                        </tr>
                    </caption>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
            Visible="false" BorderWidth="2px" Height="100px" Width="300px">
            <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                font-size: Small; font-weight: bold">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                font-weight: bold; height: 22px; font-family: 'Book Antiqua'; position: absolute;
                                top: 21px; left: 10px;"></asp:Label>
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
        <asp:TextBox ID="txtsms" runat="server" TextMode="MultiLine" Text="" Font-Names="Book Antiqua"
            Width="500px" Height="100px" Font-Size="Medium" Font-Bold="true" Visible="false"
            MaxLength="1000"></asp:TextBox>
        <br />
        <asp:Button ID="btnsendsms" runat="server" Text="Send" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" OnClick="btnsendsms_Click" Visible="false" />
        <br />
        <br />
        <asp:Panel ID="emailpanel" runat="server" Visible="false">
            <table id="Tablenote" runat="server">
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblnotification" Text="E-Mail" Font-Size="Large" Font-Names="Book Antiqua"
                            runat="server" Font-Bold="true" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            width: 90px;" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsubject" runat="server" Style="display: inline-block; color: Black;
                            font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 500px;"
                            Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblnote" Text="Content" runat="server" Font-Bold="true" Style="display: inline-block;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            width: 90px;" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtbodycontant" TextMode="MultiLine" runat="server" MaxLength="4000"
                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                            font-weight: bold; width: 500px; height: 300px;" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
            CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
            Drag="true" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel4" runat="server" Width="1000px" Height="550px" ScrollBars="Auto"
            BorderColor="Black" BorderStyle="Double" Style="display: none;">
            <div class="HellowWorldPopup">
                <div class="PopupHeader" id="Div3" style="text-align: center; color: Blue; font-family: Book Antiqua;
                    font-size: xx-large; font-weight: bold">
                </div>
                <div class="PopupBody">
                </div>
                <div class="Controls">
                    <center>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="245px" Width="950px" Visible="False" VerticalScrollBarPolicy="Never"
                            ActiveSheetViewIndex="0" Style="background-color: White;">
                            <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                    EditTemplateColumnCount="2" GridLineColor="white" GroupBarText="Drag a column to group by that column."
                                    SelectionBackColor="#EAECF5" AutoPostBack="True">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                VerticalAlign="NotSet" />
                        </FarPoint:FpSpread>
                        <center>
                            <asp:Button ID="Button1" runat="server" Text="Close" />
                            <br />
        </asp:Panel>
        <br />
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 170px;
                        width: 425px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblteachall" runat="server" Text="Teaching Allowances" Style="color: Black;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:UpdatePanel ID="updtecall" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:TextBox ID="txtteachall" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                        <asp:Panel ID="pnltecall" runat="server" CssClass="multxtpanel" Height="250px" Width="150px">
                                            <asp:CheckBox ID="cbtecall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="cbtecall_CheckedChanged" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbltecall" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cbltecall_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popuptecall" runat="server" TargetControlID="txtteachall"
                                            PopupControlID="pnltecall" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblnonteachall" runat="server" Text="Non Teaching Allowances" Style="color: Black;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:UpdatePanel ID="updnontec" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:TextBox ID="txtnonteachall" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                        <asp:Panel ID="pnlnontec" runat="server" CssClass="multxtpanel" Height="250px" Width="150px">
                                            <asp:CheckBox ID="cbnontecall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="cbnontecall_CheckedChanged" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblnontecall" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cblnontecall_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupnontecall" runat="server" TargetControlID="txtnonteachall"
                                            PopupControlID="pnlnontec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnsavestfall" CssClass=" textbox1 btn2" OnClick="btnsavestfall_Click"
                                Text="Save" runat="server" />
                            <asp:Button ID="btnexitstffall" CssClass=" textbox1 btn2" OnClick="btnexitstffall_Click"
                                Text="Exit" runat="server" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="printpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 245px;
                        width: 410px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_print" runat="server" Text="Footer Name" Style="color: Black;
                                            width: 165px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_print" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_certificate" runat="server" Text="Certificate Content" Style="color: Black;
                                            width: 165px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_certificate" runat="server" TextMode="MultiLine" Text="" Font-Names="Book Antiqua"
                                            Width="200px" Height="100px" Font-Size="Medium" Font-Bold="true" MaxLength="1000"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btn_printSave" CssClass=" textbox1 btn2" OnClick="btnsavePrint_Click"
                                Text="Save" runat="server" />
                            <asp:Button ID="btn_printexit" CssClass=" textbox1 btn2" OnClick="btnexitPrint_Click"
                                Text="Exit" runat="server" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="img_div1" runat="server" visible="false" style="height: 150em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="alertdiv" runat="server" class="table" style="background-color: White; height: auto;
                        width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblsavealert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass="textbox textbox1 btn2" Width="50px" OnClick="btnerrclose_Click"
                                                Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
    <asp:HiddenField ID="hidden2" runat="server" />
    <asp:ModalPopupExtender ID="modalpopupex1" runat="server" TargetControlID="hidden2"
        PopupControlID="jairam">
    </asp:ModalPopupExtender>
    <asp:Panel ID="jairam" runat="server" CssClass="pop" Style="display: none; height: 500;
        width: 250;" DefaultButton="btnsmsok">
        <table width="350">
            <tr class="topHandle">
                <td colspan="2" align="left" runat="server" id="td2">
                    <asp:Label ID="lblconformationmsg" runat="server" Font-Bold="True" Text="Confirmation"
                        Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60px" valign="middle" align="center">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Info-48x48.png" />
                </td>
                <td valign="middle" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblsmstechmsg" runat="server" Font-Bold="True" Text="" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnsmsok" runat="server" Text="Yes" OnClick="btnsmsok_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 170px; position: absolute;
                        top: 81px;" />
                    <asp:Button ID="btnsmscancel" runat="server" Text="No" OnClick="btnsmscancel_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 232px;
                        position: absolute; top: 81px;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hidden3" runat="server" />
    <asp:ModalPopupExtender ID="modalpoppupemail" runat="server" TargetControlID="hidden3"
        PopupControlID="panelsuper">
    </asp:ModalPopupExtender>
    <asp:Panel ID="panelsuper" runat="server" CssClass="pop" Style="display: none; height: 500;
        width: 250;" DefaultButton="btnemailok">
        <table width="350">
            <tr class="topHandle">
                <td colspan="2" align="left" runat="server" id="td1">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60px" valign="middle" align="center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Info-48x48.png" />
                </td>
                <td valign="middle" align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblemailalart" runat="server" Font-Bold="True" Text="" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnemailok" runat="server" Text="Yes" OnClick="btnemailok_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 170px;
                        position: absolute; top: 81px;" />
                    <asp:Button ID="btnemailcancel" runat="server" Text="No" OnClick="btnemailcancel_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 232px;
                        position: absolute; top: 81px;" />
                </td>
            </tr>
        </table>
        <asp:Label ID="lopdatehidden" Text="" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
