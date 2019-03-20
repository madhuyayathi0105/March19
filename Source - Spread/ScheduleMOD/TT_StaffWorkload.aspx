<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TT_StaffWorkload.aspx.cs" Inherits="AttendanceMOD_TT_StaffWorkload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .Grid
        {
            border: 2px solid #999999;
            background-color: White;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
            overflow: auto;
        }
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #pnlContents
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Staff Work Load</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green">Staff Workload Report </span>
        <br />
        <br />
        <div class="maindivstyle">
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        College
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="200px" OnSelectedIndexChanged="ddlcollege_change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Department
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddept" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                    PopupControlID="p1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Designation
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDesig" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbDesig" runat="server" Text="Select All" OnCheckedChanged="cbDesig_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblDesig" runat="server" OnSelectedIndexChanged="cblDesig_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDesig"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Staff Type
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtStfType" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 100px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbStfType" runat="server" Text="Select All" OnCheckedChanged="cbStfType_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblStfType" runat="server" OnSelectedIndexChanged="cblStfType_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtStfType"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        Staff Name
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStfName" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="200px">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Search By
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSearchOption" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" CssClass="textbox1 ddlheight5" Width="144px" OnSelectedIndexChanged="ddlSearchOption_Change"
                            AutoPostBack="true">
                            <asp:ListItem Selected="True" Text="Staff Code" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Staff Name" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="tdStfCode" runat="server" visible="false">
                        Staff Code
                    </td>
                    <td id="tdStfCodeAuto" runat="server" visible="false">
                        <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td id="tdStfName" runat="server" visible="false">
                        Staff Name
                    </td>
                    <td id="tdStfNameAuto" runat="server" visible="false">
                        <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkIncDay" runat="server" Checked="false" Text="Include Day" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkDeptDes" runat="server" Text="Include Dept and Desig" Checked="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" CssClass="btn1" Text="Go" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua"></asp:Label>
            <br />
            <asp:Panel ID="pnlContents" runat="server" Visible="false">
                <div id="printable">
                    <table style="width: 98%; height: auto; margin: 0px;">
                        <tr>
                            <td>
                                <table class="printclass" style="width: 98%; height: auto; margin: 0px;">
                                    <tr>
                                        <th align="center" colspan="6">
                                            <span id="spCollegeName" class="headerDisp" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th align="center" colspan="6">
                                            <span id="spAddr" class="headerDisp1" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th align="center" colspan="6">
                                            <span id="spReportName" class="headerDisp1" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left">
                                            <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                                        </td>
                                        <td colspan="3" align="right">
                                            <span id="spStaffName" class="headerDisp1" runat="server"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:GridView ID="grdStf_TT" runat="server" AutoGenerateColumns="True" Visible="false"
                                    OnRowDataBound="OnrowDataBoun" CssClass="Grid" GridLines="Both" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Names="Book Antiqua"
                                    HeaderStyle-Font-Size="Medium">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />
            <br />
            <asp:Button ID="btnExport" runat="server" Style="font-family: Book Antiqua; font-weight: bold;"
                Text="Export To PDF" Visible="false" OnClientClick=" return PrintPanel()" />
        </div>
    </center>
</asp:Content>
