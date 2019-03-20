<%@ Page Title="CONSOLIDATED STATEMENT OF MARKS REPORT" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ConsolidatedStatementofMarks.aspx.cs" Inherits="MarkMod_SubjectWiseTestMark" %>
    <%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lblExcelErr.ClientID %>').innerHTML = "";
        }

        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size:small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
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
 <%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <body>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">CONSOLIDATED
                STATEMENT OF MARKS REPORT</span>
        </div>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;">
            <%-- --%>
            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlCollege"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlBatch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                            AssociatedControlID="ddlDegree"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                            AssociatedControlID="ddlBranch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem" AssociatedControlID="ddlSem"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                            AutoPostBack="True" Width="40px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Section" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlSec" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="ddlTest"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <%--<asp:UpdatePanel ID="upnlTest" runat="server">
                                <ContentTemplate>--%>
                            <asp:TextBox ID="txtTest" Visible="false" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlTest" Visible="false" runat="server" CssClass="multxtpanel" Height="200px"
                                Width="280px">
                                <asp:CheckBox ID="chkTest" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                    AutoPostBack="True" OnCheckedChanged="chkTest_CheckedChanged" />
                                <asp:CheckBoxList ID="cblTest" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblTest_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popExtTest" runat="server" TargetControlID="txtTest"
                                PopupControlID="pnlTest" Position="Bottom">
                            </asp:PopupControlExtender>
                            <asp:DropDownList ID="ddlTest" runat="server" Visible="true" CssClass="commonHeaderFont"
                                OnSelectedIndexChanged="ddlTest_SelectedIndexChanged" AutoPostBack="True" Width="80px">
                            </asp:DropDownList>
                            <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblSubject" AssociatedControlID="ddlSubject" runat="server" Text="Subject"
                            CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <%--<asp:UpdatePanel ID="UpnlSubject" runat="server">
                                <ContentTemplate>--%>
                            <asp:TextBox ID="txtSubject" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlSubject" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                Width="280px">
                                <asp:CheckBox ID="chkSubject" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                    AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                <asp:CheckBoxList ID="cblSubject" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                PopupControlID="pnlSubject" Position="Bottom">
                            </asp:PopupControlExtender>
                            <asp:DropDownList ID="ddlSubject" Width="152px" Visible="false" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged" CssClass="commonHeaderFont">
                            </asp:DropDownList>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkConvertedTo" Checked="false" runat="server" CssClass="commonHeaderFont"
                            Text="Include Converted Mark" AutoPostBack="true" OnCheckedChanged="chkConvertedTo_CheckedChanged" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtConvertedMaxMark" runat="server" CssClass="commonHeaderFont"
                            Text="" Width="50px" style="margin-left:-20px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filterConvert" runat="server" TargetControlID="txtConvertedMaxMark"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td colspan="2"><asp:CheckBox ID="cbattndperc" runat="server" Text="Include Attendance%" style="margin-left:-130px" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="75px" AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="calExtFromDate" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" AutoPostBack="True" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="calExtToDate" TargetControlID="txtToDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGetMarks" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGetMarks_Click" Text="Get Marks" Style="width: auto; height: auto;" />
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkIncludeGrade" runat="server" Checked="false" CssClass="commonHeaderFont"
                            Text="Include Grade" />
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkRoundOffMarks" runat="server" Checked="false" CssClass="commonHeaderFont"
                            Text="Round off Marks" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkrange" runat="server" Checked="false" CssClass="commonHeaderFont"
                            Text="Without Range" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkSetting" runat="server" Text="Setting" OnClick="lnksettting_Click"></asp:LinkButton>
                    </td>
                </tr>
                 <tr>
                    <td align="center" colspan="2px" >
                  
                   <asp:LinkButton ID="btnsettings" runat="server" CssClass="textbox textbox1 commonHeaderFont" OnClick="btnsettings_OnClick" Text="Signature Settings" Style="width: auto; height: auto;"></asp:LinkButton>
                    </td>
                    </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 15px; margin-top: 10px;"></asp:Label>
            <center>
                <div id="divMainContents" runat="server" visible="false" style="width: auto; height: auto;">
                    <%-- style="margin: 0px; width: 100%; text-align: center;" style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px; padding: 0px; border: 0px; position: relative;"--%>
                    <table style="margin: 0px; padding: 0px; border: 0px;">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblExcelErr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblExcelReportName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExcelName" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtExcelName"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\@#$%^&*()-=+!~`<>?|:;'">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnExportExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExportExcel_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                    Text="Export To Excel" CssClass="textbox textbox1" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrintPDF" runat="server" Text="Print" OnClick="btnPrintPDF_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                    height: auto;" Visible="false" CssClass="textbox textbox1" />
                                <Insproplus:printmaster runat="server" ID="printCommonPdf" Visible="false" Style="width: auto;
                                    height: auto; left: 20%; right: 20%; top: 30%; position: fixed;" />
                            </td>
                            <td>
                            <asp:Button ID="btnDirectPrint" Visible="true" CssClass="textbox textbox1" runat="server"
                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                        height: auto;" Text="Print" OnClick="btnprintdiect" />
                            </td>
                        </tr>
                    </table>
                    <FarPoint:FpSpread ID="FpStudentMarkList" autopostback="false" runat="server" Visible="true"
                        BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                AllowPage="False" PageSize="100" SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                  <%--  <%--<asp:Button ID="btnDirectPrint" Visible="true" CssClass="textbox textbox1" runat="server"
                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                        hei--%><%--ght: auto;" Text="Direct Print" OnClick="btnprintdiect" />--%>
                </div>
            </center>
        </div>
    </center>
    <%-- Confirmation --%>
    <center>
        <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divConfirm" runat="server" class="table" style="background-color: White;
                    height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%; padding: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnYes_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnNo_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divViewSubjects" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divpnlViewSubjects" runat="server" class="table" style="background-color: White;
                    height: auto; width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Apply Subject</span>
                    </center>
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" Text="No.Of.Best" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="true" ForeColor="Black"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoofSubject" runat="server" CssClass="commonHeaderFont" Text=""
                                        Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <center>
                        <div style="width: 900px; height: auto; overflow: auto; margin: 0px; margin-bottom: 10px;
                            margin-top: 20px;">
                            <center>
                                <FarPoint:FpSpread ID="FpSpreadViewSubjects" runat="server" Height="250px" Width="400px"
                                    ActiveSheetViewIndex="0" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                        </div>
                        <asp:Label ID="lblViewSubjectError" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" ForeColor="Red" Style="margin: 0px; margin-bottom: 20px;
                            margin-top: 10px; position: relative;"></asp:Label>
                        <div style="margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;">
                            <center>
                                <asp:Button ID="btnSave" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btnSave_Clcik"
                                    Text="Save" runat="server" />
                                <asp:Button ID="btnViewSubjects_exit" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btnViewSubjects_exit_Clcik"
                                    Text="Exit" runat="server" />
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Alert Box --%>
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

     <center>
        <div id="divsettings" runat="server" visible="false" style="height: 550em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divsignsettings" runat="server" class="table" style="background-color: White;
                    height: auto; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 30%; top: 30%; position: fixed; border-radius: 10px;">
                     <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Signature Settings</span>
                    </center>
                    <center>
                        <table style="height: auto; width: 100%; padding: 5px;">
                            <tr>
                                <td > 
                                <asp:Label ID="lblfooter1" runat="server" Text="Signature" style="margin-left:24px"></asp:Label>
                                <asp:TextBox ID="txtfooter1" runat="server"  CssClass="textbox  txtheight2 commonHeaderFont"
                                        ></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr>
                                <td colspan="3px">
                                    <center>
                                        <asp:Button ID="btnsavefooter" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px; margin-top:15px"
                                            OnClick="btnsavefooter_OnClick" Text="Save" runat="server" />
                                        <asp:Button ID="btnClosefooter" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px; margin-top:15px"
                                            OnClick="btnClosefooter_OnClick" Text="Close" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>

    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>
   <%-- </body>
    </html>--%>
</asp:Content>
