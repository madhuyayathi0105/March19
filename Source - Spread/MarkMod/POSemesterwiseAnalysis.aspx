<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="POSemesterwiseAnalysis.aspx.cs" Inherits="POSemesterwiseAnalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
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
            #printdiv
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
        function printTTOutput() {
            var panel = document.getElementById("<%=printdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
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
    <style type="text/css">
        .style1
        {
            width: 890px;
            background-color: Teal;
        }
        .style2
        {
            width: 818px;
        }
        .style3
        {
            width: 150px;
        }
        .cursorptr
        {
            cursor: pointer;
        }
    </style>
    <br />
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Overall PO Chart Analysis and Report"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
    <br />
    <center>
        <table class="maintablestyle" style="width: 700px; height: 44px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="69px" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="127px" AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnClick="GO_Click" />
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <div id="printdiv" runat="server">
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
                        <span id="spRoomType" class="headerDisp1" runat="server"></span>
                    </td>
                    <td colspan="3" align="right">
                        <span id="spRoomNo" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
            <center>
                <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#0CA6CA" BackColor="White"
                    Style="margin-bottom: 15px; margin-top: 15px; width: auto;" Font-Names="Times New Roman"
                    AutoGenerateColumns="true">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>" />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </center>
        </div>
    </center>
    <br />
    <center>
        <table>
            <tr align="center">
                <td align="center">
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"
                        Visible="false"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ .">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnprint1" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnmasterprint_Click" Visible="false" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <br />
                    <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Chart ID="Chart1" runat="server" Height="800px" Width="2200px">
            <Titles>
                <asp:Title ShadowOffset="0" Name="Items" />
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                    LegendStyle="Row" />
            </Legends>
            <Series>
                <asp:Series Name="Default" />
            </Series>
            <ChartAreas>
                <%-- <asp:ChartArea Name="ChartArea1" BorderWidth="0" />--%>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX Interval="1">
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </center>
</asp:Content>
