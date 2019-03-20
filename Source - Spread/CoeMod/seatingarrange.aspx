<%@ Page Title="Exam Seating Arrangement" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="seatingarrange.aspx.cs" Inherits="seatingarrange"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        .printclass
        {
            display: none;
        }
    </style>
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
    </script>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblmessage1').innerHTML = "";
        }
        function buttoncheck() {
            var date = document.getElementById('<%=ddlDate.ClientID%>').value;
            var hall = document.getElementById('<%=ddlhall.ClientID%>').value;
            if (date == "All") {
                alert("Please Select Date");
                return false;
            }
            if (hall == "") {
                alert("Please Select Hall");
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Seating Arrangement</title>');
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
    <script type="text/javascript">
        function PrintPanel1() {
            var panel = document.getElementById("<%=pnlContent1.ClientID %>");
            var printWindow = window.open('', '', 'height= 595,width=842');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Seating Arrangement</title>');
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Exam Seating Arrangement</span>
    </center>
    <center>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="fontbold" Style="font-family: Book Antiqua;
                        font-size: large; font-weight: 700;">
                    </asp:Label>
                </td>
                <td>
                    <div style="position: relative; margin: 0px; padding: 0px;">
                        <asp:UpdatePanel ID="upnlCollege" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCollege" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Enabled="false" CssClass="Dropdown_Txt_Box fontbold" ReadOnly="true">-- Select --</asp:TextBox>
                                <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel fontbold" Style="width: 280px;
                                    height: auto;">
                                    <asp:CheckBox ID="chkCollege" Font-Names="Book Antiqua" Font-Size="Medium" runat="server"
                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged"
                                        Style="width: 100%; height: auto;" />
                                    <asp:CheckBoxList ID="cblCollege" Font-Size="Medium" Font-Names="Book Antiqua" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged"
                                        Style="width: 100%; height: auto;">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupExtCollege" runat="server" TargetControlID="txtCollege"
                                    PopupControlID="pnlCollege" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="Label20" runat="server" Style="" Width="119px" Text="Year and Month"
                        CssClass="fontbold"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" Style="" runat="server" CssClass="fontbold" Width="60px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Style="" CssClass="fontbold" Width="65px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Date" Style="" CssClass="fontbold"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDate" runat="server" Style="" CssClass="fontbold" Width="101px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Session" CssClass="fontbold" Style=""></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSession" runat="server" Style="" CssClass="fontbold" Width="90px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Type" Style="" CssClass="fontbold"></asp:Label>
                </td>
                <td colspan="2">
                    <div style="position: relative; margin: 0px; padding: 0px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddltype" runat="server" CssClass="fontbold" Style="" Width="90px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Hall No" Style="" Width="59px" CssClass="fontbold"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative; margin: 0px; padding: 0px;">
                                    <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlhall" runat="server" Enabled="false" Style="" CssClass="fontbold"
                                        Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlhall_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chksetting" Text="Display Booklet Number" Width="210px" Style=""
                                    CssClass="fontbold" Enabled="false" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbfooter" Text="Show Footer" Style="" CssClass="fontbold" Enabled="false"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="GO" Style="" CssClass="fontbold" OnClick="btngo_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnView" runat="server" Text="Generate" CssClass="fontbold" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkmergrecol" Text="Merge College" Width="130px" Checked="true"
                                    Style="" CssClass="fontbold" runat="server" AutoPostBack="true" OnCheckedChanged="chkmergrecol_CheckedChanged" />
                            </td>
                            <td colspan="2" style="width: auto; margin: 0px;">
                                <asp:CheckBox ID="chkNewSeating" Text="New Seating Arrangement" Checked="false" Style="width: 180px;
                                    margin: 0px; padding: 0px;" CssClass="fontbold" runat="server" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="Radioformat1" runat="server" Style="" CssClass="fontbold" GroupName="format"
                                    Width="100px" OnCheckedChanged="Radioformat1_CheckedChanged" Text="Format 1"
                                    AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:RadioButton ID="Radioformat2" runat="server" Style="" AutoPostBack="true" CssClass="fontbold"
                                    Width="100px" GroupName="format" OnCheckedChanged="Radioformat2_CheckedChanged"
                                    Text="Format 2" />
                            </td>
                            <td>
                                <asp:RadioButton ID="Radioformat3" runat="server" CssClass="fontbold" Style="" AutoPostBack="true"
                                    Width="100px" GroupName="format" OnCheckedChanged="Radioformat3_CheckedChanged"
                                    Text="Format 3" />
                            </td>
                            <td>
                                <div id="divShowBundleNo" visible="false" runat="server">
                                    <asp:CheckBox ID="chkShowBundleNo" runat="server" Checked="false" CssClass="fontbold"
                                        Text="Show Bundle No" />
                                </div>
                            </td>
                            <td>
                                <div id="divForCommonElective" visible="true" runat="server">
                                    <asp:CheckBox ID="chkForSeating" runat="server" Checked="false" CssClass="fontbold"
                                        Text="Seating For Common Paper" AutoPostBack="true" 
                                        oncheckedchanged="chkForSeating_CheckedChanged" />
                                </div>
                                 <div id="div1" visible="true" runat="server">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked="false" CssClass="fontbold"
                                        Text="With Qpaper order" AutoPostBack="true" />
                                </div>
                            </td>
                            <td>
                                <asp:Button ID="btnMissingStudent" runat="server" Text="Missing Student" CssClass="fontbold"
                                    OnClick="btnMissingStudent_Click" />
                            </td>
                            <td>
                                <div id="divIncludeBlock" visible="true" runat="server">
                                    <asp:CheckBox ID="chkIncludeBlock" runat="server" Checked="false" CssClass="fontbold"
                                        Text="Include Block" AutoPostBack="true" OnCheckedChanged="chkIncludeBlock_CheckedChanged" />
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblBlock" runat="server" Text="Block" CssClass="fontbold" AssociatedControlID="txtBlock"
                                    Style="font-family: Book Antiqua; font-size: large; font-weight: 700;">
                                </asp:Label>
                            </td>
                            <td>
                                <div id="divBlock" runat="server" style="position: relative; margin: 0px; padding: 0px;">
                                    <asp:UpdatePanel ID="upnlBlock" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtBlock" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Enabled="false" CssClass="Dropdown_Txt_Box fontbold" ReadOnly="true">-- Select --</asp:TextBox>
                                            <asp:Panel ID="pnlBlock" runat="server" CssClass="multxtpanel fontbold" Style="width: 280px;
                                                height: auto;">
                                                <asp:CheckBox ID="chkBlock" Font-Names="Book Antiqua" Font-Size="Medium" runat="server"
                                                    Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBlock_CheckedChanged"
                                                    Style="width: 100%; height: auto;" />
                                                <asp:CheckBoxList ID="cblBlock" Font-Size="Medium" Font-Names="Book Antiqua" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="cblBlock_SelectedIndexChanged" Style="width: 100%;
                                                    height: auto;">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popExtBlock" runat="server" TargetControlID="txtBlock"
                                                PopupControlID="pnlBlock" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" CssClass="fontbold"
            Visible="false" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
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
                            <asp:TextBox ID="txtExcelNameMissing" runat="server" CssClass="textbox textbox1"
                                Height="20px" Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                Font-Names="Book Antiqua" onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterExcelNameMissing" runat="server" TargetControlID="txtExcelNameMissing"
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
                                height: auto;" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="printCommonPdf" Visible="false" Style="width: auto;
                                height: auto; left: 20%; right: 20%; top: 30%; position: fixed;" />
                        </td>
                        <td>
                            <%--<asp:Button ID="btnDirectPrint" Visible="false" CssClass="textbox textbox1" runat="server"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                height: auto;" Text="Direct Print" OnClientClick="return PrintPanel();" />--%>
                        </td>
                    </tr>
                </table>
                <FarPoint:FpSpread ID="FpStudentList" autopostback="false" runat="server" Visible="true"
                    BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView AllowPage="False" PageSize="100" SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
        </center>
    </center>
    <center>
        <asp:Panel ID="pnlContents" runat="server" Visible="false" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <style type="text/css" media="print">
                @page
                {
                    size: A3 portrait;
                    margin: 0.5cm;
                }
                @media print
                {
                    .printclass
                    {
                        display: table;
                    }
                    thead
                    {
                        display: table-header-group;
                    }
                    tfoot
                    {
                        display: table-footer-group;
                    }
                    #header
                    {
                        position: fixed;
                        top: 0px;
                        left: 0px;
                    }
                    #footer
                    {
                        position: fixed;
                        bottom: 0px;
                        left: 0px;
                    }
                    #printable
                    {
                        position: relative;
                        bottom: 30px;
                        height: 300;
                    }
                
                }
                @media screen
                {
                    thead
                    {
                        display: block;
                    }
                    tfoot
                    {
                        display: block;
                    }
                }
            </style>
            <div id="printable">
                <table>
                    <thead>
                        <tr>
                            <th>
                                <div>
                                    <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                        font-size: medium; margin-top: 20px;">
                                        <tr>
                                            <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                                <asp:Image ID="imgLeftLogo" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                    Width="100px" Height="100px" />
                                            </td>
                                            <td align="center">
                                                <span id="spCollege" runat="server" style="font-size: 18px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spAffBy" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spController" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spSeating" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spDateSession" runat="server" style="font-size: 14px;"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <div>
                                        <asp:Table ID="tblFormat2" runat="server" Style="width: 1417px; border-color: Black;
                                            text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                            border-style: solid; border-width: 1px;">
                                            <asp:TableRow ID="tblRow1" runat="server">
                                                <asp:TableCell ID="tblcellsno" runat="server" Text="S.No" Width="30px"></asp:TableCell>
                                                <asp:TableCell ID="tblcellInvName" runat="server" Text="Invigilator Name" Width="69px"></asp:TableCell>
                                                <asp:TableCell ID="tblcellHallNo" runat="server" Text="Hall No" Width="65px"></asp:TableCell>
                                                <asp:TableCell ID="tcInvSign" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                    Width="65px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell4" runat="server" Text="Degree/<br/>Branch" Width="105px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell6" runat="server" Text="Subject Code" Width="80px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell7" runat="server" Text="Reg. No of the Candidate" Width="380px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell8" runat="server" Text="Total No of Student" Width="70px"></asp:TableCell>
                                                <asp:TableCell ID="tcBooletNo" runat="server" Text="Answer Booklet Numbers" Width="40px"></asp:TableCell>
                                                <asp:TableCell ID="tcHallSuperend" runat="server" Text="Signature <br/>of the<br/> Hall <br/>Superintendents"
                                                    Width="40px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell11" runat="server" Text="Present" Width="55px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell12" runat="server" Text="Absent" Width="55px"></asp:TableCell>
                                                <asp:TableCell ID="tcBundleNo" runat="server" Text="Bundle No" Width="55px"></asp:TableCell>
                                                <asp:TableCell ID="TableCell13" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                    Width="65px"></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="2" align="center">
                                <FarPoint:FpSpread ID="Fspread3" Visible="false" runat="server" ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
    </center>
    <center style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
        <asp:Label ID="lblreportname2" runat="server" Visible="false" Text="Report Name"
            CssClass="fontbold"></asp:Label>
        <asp:TextBox ID="txtreportname2" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            CssClass="fontbold" Visible="false" onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtreportname2"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="Exportexcel" runat="server" Visible="false" CssClass="fontbold" Width="100px"
            Text="Excel" OnClick="btn_excel" />
        <asp:Button ID="Printfspread3" runat="server" Visible="false" CssClass="fontbold"
            Width="100px" Text="Print" OnClick="btn1_print" />
        <asp:Button ID="btn_directprint" runat="server" Visible="false" CssClass="fontbold"
            Width="100px" Text="Direct Print" OnClientClick="return PrintPanel();" />
    </center>
    <%--OnClick="btn_directprint_Click"--%>
    <center>
        <asp:Panel ID="pnlContent1" runat="server" Visible="false">
            <style type="text/css" media="print">
                @page
                {
                    size: A3 portrait;
                    margin: 0.5cm;
                }
                @media print
                {
                    .printclass
                    {
                        display: table;
                    }
                    thead
                    {
                        display: table-header-group;
                    }
                    tfoot
                    {
                        display: table-footer-group;
                    }
                    #header
                    {
                        position: fixed;
                        top: 0px;
                        left: 0px;
                    }
                    #footer
                    {
                        position: fixed;
                        bottom: 0px;
                        left: 0px;
                    }
                    #printable1
                    {
                        position: relative;
                        bottom: 30px;
                        height: 300;
                        width: 100%;
                    }
                
                }
                @media screen
                {
                    thead
                    {
                        display: block;
                    }
                    tfoot
                    {
                        display: block;
                    }
                }
            </style>
            <div id="printable1">
                <table width="100%">
                    <thead>
                        <tr>
                            <th colspan="2">
                                <div>
                                    <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                        font-size: medium; margin-top: 20px;">
                                        <tr>
                                            <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                                <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                    Width="100px" Height="100px" />
                                            </td>
                                            <td align="center">
                                                <span id="spF1College" runat="server" style="font-size: 18px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spF1Aff" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spF1Controller" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spF1Seating" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span id="spExamination" runat="server" style="font-size: 15px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <span id="spF1Date" runat="server" style="font-size: 14px;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <span id="spHallNo" runat="server" style="font-size: 14px;"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <asp:Table ID="tblFormat1" runat="server" Style="border-color: Black; text-align: center;
                                        border-bottom: 0px solid black; font-weight: bold; font-size: medium; border-style: solid;
                                        border-width: 1px; margin: 0px;">
                                        <asp:TableHeaderRow ID="tblHeader1" runat="server" Style="width: auto; border-color: Black;
                                            text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                            border-style: solid; border-width: 1px; margin: 0px;">
                                        </asp:TableHeaderRow>
                                        <asp:TableHeaderRow ID="tblHeader2" runat="server" Style="width: auto; border-color: Black;
                                            text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                            border-style: solid; border-width: 1px; margin: 0px;">
                                        </asp:TableHeaderRow>
                                    </asp:Table>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <center>
                                    <FarPoint:FpSpread ID="Fpspread" Visible="false" runat="server" ShowHeaderSelection="false">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
    </center>
    <center>
        <FarPoint:FpSpread ID="Fpseating" Visible="false" runat="server" autopostback="true"
            Width="980px" ShowHeaderSelection="false" Style="margin: 0px; margin-bottom: 10px;
            position: relative; left: 0%; width: auto; height: auto;">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <center style="margin: 0px; margin-bottom: 10px; margin-top: 15px; position: relative;">
        <asp:Label ID="lblexcsea" runat="server" Visible="false" Text="Report Name" CssClass="fontbold"></asp:Label>
        <asp:TextBox ID="txtexseat" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            CssClass="fontbold" Visible="false" onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexseat"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="Excel_seating" runat="server" Visible="false" Text="Export Excel"
            CssClass="fontbold" OnClick="Excelseating_click" />
        <asp:Button ID="Print_seating" runat="server" CssClass="fontbold" Visible="false"
            Text="Print" OnClick="printseating_click" />
    </center>
    <center>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblmessage1" runat="server" CssClass="fontbold" ForeColor="#FF3300"
                        Text="" Visible="False">
                    </asp:Label>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 15px;">
            <tr>
                <td>
                    <asp:Label ID="lblrptname" runat="server" Visible="false" Text="Report Name" CssClass="fontbold"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        CssClass="fontbold" Visible="false" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Visible="false" Text="Export Excel" CssClass="fontbold"
                        OnClick="btnxl_Click" OnClientClick=" return buttoncheck()" />
                    <asp:Button ID="btnprintmaster" runat="server" Visible="false" Text="Print" CssClass="fontbold"
                        OnClick="btnprint_Click" />
                    <asp:Button ID="btnDirectPrint" runat="server" CssClass="fontbold" Visible="false"
                        Text="Direct Print" OnClientClick="return PrintPanel1();" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
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
</asp:Content>
