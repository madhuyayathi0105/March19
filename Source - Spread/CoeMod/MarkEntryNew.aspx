<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MarkEntryNew.aspx.cs" Inherits="MarkEntryNew" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="position: relative; margin: 0px; margin-bottom: 25px; width: 100%; height: auto;">
        <asp:Panel ID="header_Panel" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="width: 1040px; height: 21px">
            <center>
                <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="Exam Mark Subject Wise Report"></asp:Label>
            </center>
        </asp:Panel>
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="chkmergrecol" runat="server" Text="Mergre College" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Year And Month"></asp:Label>
                    <asp:DropDownList ID="ddlYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged" Font-Size="Medium" Width="60px"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="font" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True">
                    </asp:DropDownList>
                    <%--</td>
            <td runat="server" id="degrrt" colspan="2" align="left">--%>
                    <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:DropDownList ID="ddltype" runat="server" Width="128px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:DropDownList ID="ddlbatch" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:DropDownList ID="ddldegree1" runat="server" CssClass="font" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    &nbsp;&nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Branch"></asp:Label>
                    <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="font" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="160px" OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Sem"></asp:Label>
                    <asp:DropDownList ID="ddlsem1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                    <asp:DropDownList ID="ddlsubtype" AutoPostBack="true" Width="200px" runat="server"
                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                    <asp:DropDownList ID="ddlSubject" AutoPostBack="true" Width="407px" runat="server"
                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chk_onlycia" runat="server" Enabled="false" Text="I.C.A Mark" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True" />
                    <asp:CheckBox ID="chksubwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                    <asp:CheckBox ID="chkicaretake" runat="server" Text="ICA Repeat/Retake" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                    <asp:CheckBox ID="chkonlyica" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Only ICA" />
                    <asp:CheckBox ID="chkIIIvaluation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="III Valuation Only" />
                    <asp:Button ID="btnviewre" runat="server" Text="View Report" OnClick="btnviewre_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 1040px;
            height: 10px">
        </asp:Panel>
        <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
        <table width="830px">
            <tr>
                <td>
                    <asp:RadioButton ID="rbeval" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Valuation" GroupName="Import" />
                    <asp:RadioButton ID="rbcia" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="I.C.A" GroupName="Import" />
                    <asp:FileUpload runat="server" ID="fpmarkexcel" />
                    <asp:Button ID="btnexcelimport" runat="server" Font-Bold="true" OnClick="btnexcelimport_click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Import" />
                    &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:CheckBox ID="chkincluevel2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Maroon" Text="For Single Valuation Only" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnsave1" runat="server" Font-Bold="true" OnClick="btnsavel1_click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Save" />
                    <asp:Button ID="btnprintt" runat="server" Font-Bold="true" OnClick="btnprintt_print"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Print Type I" />
                    <asp:Button ID="btnprintt_t1" runat="server" Font-Bold="true" OnClick="btnprintt_t1_print"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Print Type II" />
                    <asp:Button ID="btnreset" runat="server" Font-Bold="true" OnClick="btnreset_print"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Reset" />
                    <asp:Button ID="btnPrint" runat="server" Font-Bold="true" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="fontblack" Text="Print III Val." OnClientClick="return PrintPanel();" />
                    <asp:CheckBox runat="server" ID="chkmoderation" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium" Text="Apply Moderation" ForeColor="Maroon" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblaane" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If  AB: Absent, NR: Not Registered, NE:Not Entered, M: Mal Practice, LT: Discontinue"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
            ShowHeaderSelection="false" BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never"
            HorizontalScrollBarPolicy="Never" CssClass="stylefp">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
        <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
            EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
            HorizontalScrollBarPolicy="Never" CssClass="stylefp">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                    SelectionForeColor="White">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </div>
    <center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 200%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                    width: 507px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <br />
                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btn_errorclose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <br />
                </div>
            </center>
        </div>
    </center>
    <style>
        #print1
        {
            display: none;
        }
    </style>
    <div id="print1">
        <style type="text/css" media="print">
            @page
            {
                size: A3 portrait;
                margin: 0.5cm;
            }
            
            @media print
            {
                #print1
                {
                    display: block;
                }
            }
        </style>
        <table class="style1" style="font-family: book antiqua; font-size: medium; font-weight: bold;">
            <thead>
                <tr>
                    <td rowspan="3">
                        <img id="img_leftlogo" src="college/Left_Logo.jpeg" style="height: 100px;" />&nbsp;
                    </td>
                    <td colspan="3" rowspan="3">
                        <center>
                            <asp:Label ID="lblcolname" Style="font-family: book antiqua; font-size: 20px; font-weight: bold;"
                                runat="server"></asp:Label>&nbsp;
                            <br />
                            <asp:Label ID="lbl_con" Style="font-family: book antiqua; font-size: 15px; font-weight: bold;"
                                runat="server">OFFICE OF THE CONTROLLER OF EXAMINATIONS</asp:Label>
                            <br />
                            <asp:Label ID="lbl_tit" Style="font-family: book antiqua; font-size: 15px; font-weight: bold;"
                                runat="server">Third Valuation Eligible Student Report</asp:Label></center>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;">
                        Batch Year
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblbatchshow" Style="font-family: book antiqua; font-size: medium;
                            font-weight: bold;" runat="server"></asp:Label>&nbsp;
                    </td>
                    <td align="left" style="width: 121px;">
                        Semester
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblsemestershow" Style="font-family: book antiqua; font-size: medium;
                            font-weight: bold;" runat="server"></asp:Label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Branch
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblbranchshow" Style="font-family: book antiqua; font-size: medium;
                            font-weight: bold;" runat="server"></asp:Label>&nbsp;
                    </td>
                    <td>
                        Month &amp; Year
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblexamm_y_show" Style="font-family: book antiqua; font-size: medium;
                            font-weight: bold;" runat="server"></asp:Label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <center>
                            <asp:Label ID="lblsubject_nameshow" Style="font-family: book antiqua; font-size: medium;
                                font-weight: bold;" runat="server"></asp:Label>&nbsp;
                        </center>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table class="style1" style="text-align: center; border: 1px solid black; font-weight: bold;
                            padding: 6px;">
                            <tr>
                                <td style="width: 50px;">
                                    S.No
                                </td>
                                <td style="width: 100px;">
                                    Roll No.
                                </td>
                                <td style="width: 100px;">
                                    Val I
                                </td>
                                <td style="width: 100px;">
                                    Val II
                                </td>
                                <td style="width: 100px;">
                                    Difference
                                </td>
                                <td style="width: 100px;">
                                    Remarks
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="6">
                        <asp:PlaceHolder ID="inerttablethirdval" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="6" style="text-align: right">
                        <span id="span7" runat="server" class="fontb">
                            <br />
                            <br />
                            <br />
                            <br />
                            CONTROLLER OF EXAMINATIONS </span>
                    </td>
                </tr>
            </tfoot>
        </table>
        <style type="text/css">
            .style1 tbody
            {
                border-collapse: collapse;
            }
            
            .style1
            {
                border-collapse: collapse;
                width: 100%;
            }
            
            .style1in tbody th
            {
                border: 1px solid black;
            }
            .style1in tbody td
            {
                border: 1px solid black;
            }
            .style1in tbody
            {
                border-collapse: collapse;
            }
            
            .style1in
            {
                text-align: center;
                border-collapse: collapse;
                width: 100%;
            }
            
            .style2
            {
                width: 7px;
            }
        </style>
    </div>
    <script type="text/javascript">
        function PrintPanel() {

            var panel = document.getElementById("print1");
            var printWindow = window.open('', '', 'height=566,width=880');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>DIV Contents</title>');
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
