<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamTimeTableAlter.aspx.cs" Inherits="ExamTimeTableAlter"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
        }
    </script>
     <script type="text/javascript">
         function funCalled() {
             if (CheckBox1.checked) {
             TextBox1.style.display="block"
    } else {
     TextBox1.style.display="none"

    }
         }
    </script>
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 155px;
            right: 5px;
            position: absolute;
            font-weight: bold;
            width: 100%;
            height: 25px;
            left: 5px;
        }
        .head1
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            left: 5px;
            right: 5px;
            top: 300px;
            position: absolute;
            font-weight: bold;
            width: 100%;
            height: 25px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 980px;
            position: absolute;
            height: 80px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
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
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            ForeColor="Green" Font-Size="Large" Text="Exam Time Table Alternate"></asp:Label></center>
    <br />
    <center>
        <table class="maintablestyle" style="height: 70px; background-color: #0CA6CA;">
            <tr>
                <td colspan="9">
                    <asp:Label ID="lblMonthandYear" runat="server" Text="Month and Year" CssClass="font"
                        Width="125px"></asp:Label>
                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        CssClass="font">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        CssClass="font">
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbDate" runat="server" CssClass="font" Text="Date" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="cbDate_CheckedChanged" />
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtFromDate_TextChanged"
                        AutoPostBack="true"></asp:TextBox>
                    <asp:CalendarExtender ID="cetxtexamFromdate" runat="server" TargetControlID="txtFromDate"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtToDate_TextChanged"
                        AutoPostBack="true"></asp:TextBox>
                    <asp:CalendarExtender ID="cetxtExamToDate" runat="server" TargetControlID="txtToDate"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:CheckBox ID="chkCollege" runat="server" CssClass="font" Text="College" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged" Width="90px" />
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpnlCollege" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCollege" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                    <asp:CheckBox ID="chkallColleges" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkallColleges_CheckedChanged" />
                                    <asp:CheckBoxList ID="cblCollege" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popubExtCollege" runat="server" TargetControlID="txtCollege"
                                    PopupControlID="pnlCollege" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:CheckBox ID="cbBatchYear" runat="server" CssClass="font" Text="Batch/Year" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="cbBatchYear_CheckedChanged" Width="100px" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatchYear" runat="server" CssClass="font" Width="70px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlBatchYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <asp:Label ID="lblType" runat="server" CssClass="font" Text="Stream"></asp:Label>
                    <asp:DropDownList ID="ddltype" runat="server" CssClass="font" Width="80px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbCourse" runat="server" CssClass="font" Text="Course" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="cbCourse_CheckedChanged" />
                    <asp:DropDownList ID="ddlCourse" runat="server" CssClass="font" Width="80px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbDepartment" runat="server" CssClass="font" Text="Degree" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="cbDepartment_CheckedChanged" />
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="font" Width="150px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbSubject" runat="server" CssClass="font" Text="Subject Name" TextAlign="Right"
                        AutoPostBack="True" OnCheckedChanged="cbSubject_CheckedChanged" Width="125px" />
                    <asp:DropDownList ID="ddlSubjectName" runat="server" CssClass="font" Width="150px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlSubjectName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <asp:CheckBox ID="chkindegee" Text="Include Degree Details" AutoPostBack="True" OnCheckedChanged="chkindegee_CheckedChanged"
                        runat="server" CssClass="font" />
                    <asp:Button ID="btnView" runat="server" Text="Go" CssClass="font" OnClick="btnView_Click" />
                    <asp:Button ID="btnmissingsubject" runat="server" Text="Missing Subject" CssClass="font"
                        OnClick="btnmissingsubject_Click" />
                    <asp:Button ID="btnduplicatecheck" runat="server" Text="Duplicate Check" CssClass="font"
                        OnClick="btnduplicatecheck_Click" />
                    <asp:Button ID="btnadd" runat="server" Text="ADD SUBJECT IN TIME TABLE" CssClass="font"
                        OnClick="btnadd_Click" />
                    <asp:Button ID="btndeletesubject" runat="server" CssClass="font" OnClick="btndeletesubject_OnClick" 
                    Text="Delete" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblerror" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
        <br />
        <FarPoint:FpSpread ID="Fptimetable" runat="server" ActiveSheetViewIndex="0" BorderColor="Black"
            BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" CssClass="cursorptr"
            currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
            Height="212px" ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua" ScrollBarBaseColor="Goldenrod"
            ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66" scrollContent="true"
            scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500" OnCellClick="Fptimetable_CellClick"
            OnPreRender="Fptimetable_SelectedIndexChanged" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" Visible="False">
                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
            </CommandBar>
            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" />
            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" />
            <Sheets>
                <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                    PageSize="100" SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
            </TitleInfo>
        </FarPoint:FpSpread>
        <br />
        <div id="divMove" runat="server">
            <table>
                <tr>
                    <td colspan="4">
                    <div id="divInsideMove" runat="server" visible="true">
                        <asp:Label ID="lblmedate" runat="server" Text="Exam Date" CssClass="font"></asp:Label>
                        <asp:DropDownList ID="ddlmedate" runat="server"  CssClass="font"  OnSelectedIndexChanged="ddlmedate_SelectedIndexChanged" autopostback="true">
                        </asp:DropDownList>
                        <%-----magesh2/2/18   -------%> 
                         <asp:TextBox ID="TextBox1" runat="server" Visible="false"   CssClass="font" Width="80px" OnTextChanged="txtToDate_TextChanged"
                        ></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenddlate" runat="server"  TargetControlID="TextBox1"
                        Format="dd/MM/yyyy">  
                        </asp:CalendarExtender><%-----magesh2/2/18   -------%> 
                        <asp:Label ID="lblmesession" runat="server" Text="Exam Session" CssClass="font"></asp:Label>
                        <asp:DropDownList ID="ddlmesession" runat="server" CssClass="font">
                        </asp:DropDownList>
                        <asp:Button ID="btnmemove" runat="server" Text="Move" CssClass="font" OnClick="btnmemove_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
              <td colspan="3"  align="right" > 
              <asp:Panel ID="panelchkdelete" Visible="false" runat="server">
                <asp:CheckBox id="chckdeletesubjects" runat="server" Visible="true" Text="Delete Subjects" AutoPostBack="true" OnCheckedChanged="chckdeletesubjects_OnCheckedChanged" />
                </asp:Panel>
                </td>
                <td>
                <asp:Panel ID="panelbtndeletesubject" Visible="false" runat="server">
                <asp:Button ID="btndeletesubjectold" runat="server" Visible="false" OnClick="btndeletesubject_OnClick" Text="Delete" />
                </asp:Panel>
                </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:Panel ID="treepanel" runat="server" BorderStyle="Dotted" BorderColor="ActiveBorder">
            <FarPoint:FpSpread ID="FpMissingSubject" runat="server" Height="222px" Width="565px"
                ActiveSheetViewIndex="0" currentPageIndex="0" OnUpdateCommand="FpMissingSubject_UpdateCommand"
                ShowHeaderSelection="false">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                </CommandBar>
                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False"></Pager>
                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False"></HierBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="#DEDFDE"
                        SelectionBackColor="#CE5D5A" SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                    Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False">
                </TitleInfo>
            </FarPoint:FpSpread>
            <br />
            <asp:Label ID="lbledate" runat="server" Text="Exam Date" CssClass="font"></asp:Label>
            <asp:DropDownList ID="ddledate" runat="server" CssClass="font">
            </asp:DropDownList>
            <asp:Label ID="lblesession" runat="server" Text="Exam Date" CssClass="font"></asp:Label>
            <asp:DropDownList ID="ddlesession" runat="server" CssClass="font">
            </asp:DropDownList>
            <asp:Button ID="btnalter" runat="server" Text="Save" CssClass="font" OnClick="btnalter_Click" />
        </asp:Panel>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
            onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Clcik" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <asp:Panel ID="PSubAddtt" runat="server" BorderColor="Black" BackColor="AliceBlue"
            Visible="false" BorderWidth="2px" Style="left: 223px; top: 186px; position: absolute;">
            <table style="font-family: Book Antiqua; font-size: medium; position: relative; margin-top: 10px;
                margin-bottom: 5px;">
                <tr>
                    <td>
                        <asp:Label ID="lblptype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Mode"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpmode" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpmode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblpedu" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Education"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpedu" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpedu_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblpsem" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Sem"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpsem" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkIncludeAlreadyAllotedSubjects" runat="server" CssClass="font"
                            Text="Include Already Generated Subjects" AutoPostBack="true" OnCheckedChanged="chkIncludeAlreadyAllotedSubjects_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblpsubtype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Subject Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubtype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblpsubject" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Subject"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtpsubject" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psubject" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" OnCheckedChanged="chksubject_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklssubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubject_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtpsubject"
                                    PopupControlID="psubject" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblpdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Exam Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtpdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtpdate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblpsession" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Exam Session"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpsession" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true">
                        </asp:DropDownList>
                    </td>
                    <tr>
                    <td><asp:Label ID="lblStuType" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Student Type"></asp:Label></td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtType" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkType" runat="server" Font-Bold="True" OnCheckedChanged="chkType_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cblType" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblType_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Regular</asp:ListItem>
                                        <asp:ListItem Value="1">Arrear</asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtType"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnpset" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Set" OnClick="btnpset_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnexit" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Exit" OnClick="btnexit_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblperror" runat="server" Text="Exam Date" ForeColor="Red" Visible="false"
                CssClass="font"></asp:Label>
        </asp:Panel>
    </center>
       <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
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
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
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
</asp:Content>
