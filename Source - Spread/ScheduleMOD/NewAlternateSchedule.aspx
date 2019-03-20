<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewAlternateSchedule.aspx.cs" Inherits="ScheduleMOD_NewAlternateSchedule" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 84%;
        }
        .cursorptr
        {
            cursor: pointer;
        }
        .cursordflt
        {
            cursor: default;
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
        
        #clsbtn
        {
            height: 26px;
            width: 72px;
        }
        
        .txt
        {
        }
        
        .style8
        {
            width: 319px;
        }
        
        
        .style12
        {
            width: 660px;
        }
        
        .style13
        {
            width: 133px;
        }
        .style14
        {
            width: 92px;
        }
    </style>
    <%--<script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";
        }
        function DisplayLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "block";
        }
        function HideLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "none";
        }

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Master Alternate Schedule Change" class="fontstyleheader"
            Style="color: #008000; font-size: x-large"></asp:Label>
    </center>
    <div>
        <center>
            <table cellpadding="0px" cellspacing="0px" style="width: 800px; height: 70px; background-color: #0CA6CA;"
                class="table">
                <tr style="height: 47px;">
                    <td style="padding-left: 15px;">
                        <asp:Label runat="server" ID="lblClg" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="320px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_batch" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" Font-Size="Medium"
                                    Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                    height: auto;">
                                    <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="panel_batch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtBranch" runat="server" Style="height: 20px; width: 100px;" Font-Size="Medium"
                                    Font-Bold="True" ReadOnly="true" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel3" runat="server" CssClass="multxtpanel" Style="width: 350px;
                                    height: auto;">
                                    <asp:CheckBox ID="chkBranch" runat="server" Width="140px" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="chkBranch_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cblBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblBranch_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtBranch"
                                    PopupControlID="panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                    <td>
                        <asp:Label ID="lbldate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" CssClass="txt" runat="server" Height="20px" Width="79px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox><%--OnTextChanged="txtFromDate_TextChanged"--%>
                        <asp:FilteredTextBoxExtender ID="txtDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                            ValidChars="/" runat="server" TargetControlID="txtDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="caldate" TargetControlID="txtDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="28px" Width="37px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" style="margin-right: 10px;">
                        <div runat="server" id="subDiv">
                            <table cellpadding="0px" cellspacing="0px" style="width: 500px; height: 45px; background-color: #0CA6CA;"
                                class="table">
                                <tr>
                                    <td style="padding-right: 10px;" colspan="2">
                                        <span style="overflow: hidden">
                                            <asp:Button ID="btnFreeStaffList" runat="server" Text="Free Staff List" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="28px" Width="200px" Visible="false"
                                                Style="margin: 14px;" />
                                        </span>
                                        <asp:Button ID="btnBatchAllocation" runat="server" Text=" Batch Allocation" Font-Bold="True"
                                            OnClick="btnBatchAllocation_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Style="margin: 14px;" Height="28px" Width="200px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <center>
        </center>
        <br />
        <br />
        <center>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="norecordlbl" runat="server" Style="margin-top: -25px; position: absolute;"
                            Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <%-----------------------------------------Free STaff list pop up------------------------------------------------%>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFreeStaffList"
                CancelControlID="Button1" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
                Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double" Style="display: none;
                height: 400; width: 700;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book Antiqua;
                        font-size: xx-large; font-weight: bold">
                        Free Staff List</div>
                    <div class="PopupBody">
                    </div>
                    <div class="Controls">
                        <center>
                            <FarPoint:FpSpread ID="freestaff" runat="server" AllowSort="true" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                Height="287px" Width="750" ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua"
                                ScrollBarBaseColor="Goldenrod" ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66"
                                scrollContent="true" scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500"
                                ActiveSheetViewIndex="0">
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
                                    <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;AllowSort&gt;True&lt;/AllowSort&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                        PageSize="100" SheetName="Sheet1" AllowSort="True">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                                    HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                        </center>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="Exit" />
                    </div>
                </div>
            </asp:Panel>
            <%-------------------------------------------------------------------------------------------------------------------%>
            <%---------------------------------------------As per day schedule---------------------------------------------------%>
            <span style="float: right">
                <asp:CheckBox ID="chkPerDAySched" runat="server" Width="200px" Text="As per day schedule"
                    OnCheckedChanged="chkPerDAySched_OnCheckedChanged" AutoPostBack="true" Visible="false" />
                <asp:Button ID="sem_schedule" runat="server" Text="As per day schedule" Style="border-bottom-style: none;"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ToolTip="Select any cell from Alternate schedule column" />
                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="sem_schedule"
                    CancelControlID="Button6" PopupControlID="Panel2" PopupDragHandleControlID="PopupHeader"
                    Drag="true" BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double" Style="display: none;
                    height: 250; width: 700;">
                    <div class="HellowWorldPopup">
                        <div class="PopupHeader" id="Div1" style="text-align: center; color: Blue; font-family: Book Antiqua;
                            font-size: xx-large; font-weight: bold">
                            Semester schedule</div>
                        <div class="PopupBody">
                        </div>
                        <div class="Controls">
                            <br />
                            <br />
                            <FarPoint:FpSpread ID="semspread" runat="server" Height="263px" Width="901px" OnCellClick="semspread_CellClick"
                                OnPreRender="semspread_SelectedIndexChanged" ActiveSheetViewIndex="0" currentPageIndex="0"
                                DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                EnableClientScript="False" BorderStyle="Double" BorderWidth="2px" Visible="false">
                                <CommandBar BackColor="Control">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="#DEDFDE"
                                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                        SelectionForeColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                            <center>
                                <asp:Label ID="semmsglbl" runat="server" Text="Select any cell" ForeColor="Red" Font-Size="Larger"></asp:Label>
                            </center>
                            <br />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button6" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="75px" Style="margin-top: 15px;" />
                        </div>
                    </div>
                </asp:Panel>
            </span>
            <br />
            <br />
            <%-- ----------------------------------------------------------------------------------------------%>
            <div id="divSpreadDet" runat="server" visible="false" style="overflow: auto; border: 1px solid Gray;
                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    OnPreRender="spreadDet_SelectedIndexChanged" BorderWidth="0px" Style="overflow: auto;
                    border: 0px solid #999999; border-radius: 10px;" OnCellClick="spreadDet_CellClick">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Cyan">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
        </center>
        <center>
            <div id="spcellClickPopup" runat="server" visible="false" style="height: 50em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: fixed; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 38px; margin-left: 500px;"
                    OnClick="spcellClickPopupclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 1050px;
                    height: 550px;" align="center">
                    <center>
                        <br />
                        <asp:Label ID="lblalter" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Alter Schedule"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="width: 1000px; height: 400px; border-radius: 10px; border: 1px solid Gray;">
                        <table>
                            <tr>
                                <td>
                                    <div align="left" style="overflow: auto; width: 392px; height: 392px; border-radius: 10px;
                                        border: 1px solid Gray;">
                                        <asp:TreeView runat="server" ID="subjtree" BackColor="White" Height="300px" Width="300px"
                                            SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="LightBlue" AutoPostBack="true"
                                            OnSelectedNodeChanged="subjtree_SelectedNodeChanged" Font-Names="Book Antiqua"
                                            Font-Size="Small" ForeColor="Black">
                                        </asp:TreeView>
                                    </div>
                                </td>
                                <td style="width: 40px;">
                                </td>
                                <td runat="server" id="altersp_td" visible="false" class="style12">
                                    <br />
                                    <br />
                                    <table style="margin-top: -57px; float: right;">
                                        <tr runat="server" id="tr_mulstaff" style="visibility: visible;">
                                            <td colspan="2" style="padding-left: 20px; height: 50px; padding-top: 10px;">
                                                <asp:Label ID="lblmulstaff" runat="server" Text="For Multiple Staff Selection Only"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true">
                                                </asp:Label>
                                                <asp:TextBox ID="txtmulstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                    Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="pmulstaff" runat="server" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkmulstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        OnCheckedChanged="chkmulstaff_ChekedChange" Font-Size="Medium" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chkmullsstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chkmullsstaff_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtmulstaff"
                                                    PopupControlID="pmulstaff" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <asp:Button ID="btnmulstaff" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                                                    OnClick="btnmulstaff_Click" Font-Size="Medium" />
                                                   

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <FarPoint:FpSpread ID="fpSpreadTreeNode" runat="server" Height="222px" Width="565px"
                                                    ActiveSheetViewIndex="0" currentPageIndex="0" OnButtonCommand="fpSpreadTreeNode_ButtonCommand"
                                                    OnCellClick="fpSpreadTreeNode_CellClick">
                                                    <%-- <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                        ButtonShadowColor="ControlDark">
                                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                                    </CommandBar>--%>
                                                    <%--<Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" />
                                                    <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" />
                                                    <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False"></Pager>
                                                    <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False"></HierBar>--%>
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="#DEDFDE"
                                                            SelectionForeColor="White">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                    <%--<TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                        Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False">
                                                    </TitleInfo>--%>
                                                </FarPoint:FpSpread>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="float: right; padding-top: 15px;">
                                                <asp:CheckBox ID="chkForAlternateStaff" runat="server" Text="For Alternate Staff"
                                                    Checked="false" />
                                            </td>
                                            <td style="padding-left: 40px; padding-top: 15px;">
                                                <asp:CheckBox ID="chkappend" runat="server" Text="Append to the schedule List" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="blue" OnCheckedChanged="chkSelectAlterStaff_CheckedChanged" AutoPostBack="true"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <center>
                        <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </center>
                    <div style="overflow: auto; float: right; margin-right: 25px; margin-top: 25px;">
                        <asp:Button ID="btnOk" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="75px" OnClick="btnOk_Click" />
                    </div>
                </div>
            </div>
        </center>
        <%--Free Staff list popup--%>
        <center>
            <div id="divAlterFreeStaffDetails" runat="server" visible="false" style="height: 160em;
                z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0px;">
                <center>
                    <div id="divAlterFreeStaff" runat="server" class="table" style="background-color: White;
                        height: 600px; width: 85%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        top: 1%; left: 5%; right: 5%; position: fixed; border-radius: 10px;">
                        <center>
                            <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                                margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Free Staff
                                List</span>
                        </center>
                        <div>
                            <asp:Label ID="lblAlterDate" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblAlterHour" runat="server" Text="" Visible="false"></asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAlterFreeCollege" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAlterFreeCollege" runat="server" OnSelectedIndexChanged="ddlAlterFreeCollege_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAlterFreeDepartment" Text="Department" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAlterFreeDepartment" Width="100px" runat="server" OnSelectedIndexChanged="ddlAlterFreeDepartment_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSearchBy" runat="server" Text="Staff By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAlterFreeStaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlAlterFreeStaff_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAlterFreeStaffSearch" runat="server" OnTextChanged="txtAlterFreeStaffSearch_TextChanged"
                                            Width="200px" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <center>
                            <div style="width: auto; height: auto; overflow: auto; margin: 0px; margin-bottom: auto;
                                margin-top: auto">
                                <center>
                                    <FarPoint:FpSpread ID="FpAlterFreeStaffList" runat="server" Height="250px" Width="500px"
                                        OnButtonCommand="FpAlterFreeStaffList_ButtonCommand" ShowHeaderSelection="false"
                                        ActiveSheetViewIndex="0" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </div>
                            <asp:Label ID="lblAlterFreeStaffError" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="false" ForeColor="Red" Style="margin: 0px; margin-bottom: 20px;
                                margin-top: 10px; position: relative;">
                            </asp:Label>
                            <asp:Button ID="btnSelectStaff" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="75px" Style="margin-top: 15px;" OnClick="btnSelectStaff_Click" /><%----%>
                            <asp:Button ID="btnFreeStaffExit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="75px" Style="margin-top: 15px;" OnClick="btnFreeStaffExit_Click" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </div>
    <br />
    <br />
    <div>
        <center>
            <span>
                <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="25px" Width="75px" Visible="false" OnClick="btnsave_Click" />
            </span>
        </center>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel>
                <div id="Div3" runat="server" visible="false" style="height: 200%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 160px;
                            width: 530px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 551px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label12" runat="server" Text="" Style="color: Black;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnOKsave" runat="server" Text="Ok" CssClass="btn1 textbox1 textbox "
                                                    Width="70px" OnClick="btnOKsave_Clik" />
                                                <asp:Button ID="bt_closedalter" runat="server" Text="Cancel" CssClass="btn1 textbox1 textbox "
                                                    OnClientClick="return DisplayLoadingDiv();" OnClick="bt_closedalter_Clik" Width="80px" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
