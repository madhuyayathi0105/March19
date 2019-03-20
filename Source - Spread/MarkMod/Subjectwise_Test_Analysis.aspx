<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Subjectwise_Test_Analysis.aspx.cs" Inherits="Subjectwise_Test_Analysis"
    EnableEventValidation="false" EnableViewState="true" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmgr" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl" runat="server" Text="Overall Subject Wise Test Analysis Report"
            Font-Bold="true" Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="Upanel3" runat="server">
        <ContentTemplate>
            <div style="width: 996px; height: auto; background-color: -webkit-border-radius: 10px;
                -moz-border-radius: 10px; padding: 10px; padding-left: auto; padding-right: auto;
                margin: -159px  auto auto 0px; background-color: #219DA5;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="Medium" Width="70px" Text="College" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_collage" runat="server" Width="213px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_collage_OnSelectedIndexChanged"
                                AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Batch" runat="server" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Batch" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Batch" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddl_Batch_OnSelectedIndexChanged"
                                AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Degree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Degree" ForeColor="#ffffff" Style="width: auto;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_degree" runat="server" Width="110px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddl_degree_OnSelectedIndexChanged"
                                AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Branch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Branch" ForeColor="#ffffff" Style="width: auto;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Branch" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddl_Branch_OnSelectedIndexChanged"
                                AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Sem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Semester" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlSem" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Width="90px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlsem" runat="server" CssClass="multxtpanel" Style="width: auto;
                                            margin: 0px;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sem_OnCheckedChanged" Font-Bold="True" ForeColor="Black"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; width: auto;" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Style="display: -moz-inline-box; width: 100%;">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_sem" runat="server" TargetControlID="txtSem" PopupControlID="pnlsem"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <asp:DropDownList ID="ddl_Sem" runat="server" Visible="false" Width="60px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_Sem_OnSelectedIndexChanged"
                                            AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Sec" runat="server" Font-Bold="true" Width="70px" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Section" ForeColor="#ffffff"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Sec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddl_Sec_OnSelectedIndexChanged" AutoPostBack="true"
                                            Style="border-radius: 2px; width: 50px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Subjects" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Subjects" ForeColor="#ffffff" Width="80px"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="width: 130px;">
                                            <asp:UpdatePanel ID="upnlSubjects" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_subject" runat="server" Width="100px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_subject_OnSelectedIndexChanged"
                                                        AutoPostBack="true" Style="border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTest" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Test" ForeColor="#ffffff" Width="80px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upnlTest" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="Txt_Test" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Width="100px" runat="server" Font-Size="Medium" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel_test" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                    <asp:CheckBox ID="Cb_test" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="Cb_test_CheckedChanged" />
                                                    <asp:CheckBoxList ID="Cbl_test" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" AutoPostBack="True" OnSelectedIndexChanged="Cbl_test_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="Txt_Test"
                                                    PopupControlID="Panel_test" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <td colspan="4">
                                                <asp:Button ID="btngo" runat="server" Style="background-color: silver; border: 2px solid white;
                                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                    height: 27px; width: 42px; border-radius: 2px; -webkit-border-radius: 2px; -moz-border-radius: 2px;"
                                                    Text="Go" OnClick="btngo_Click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="margin-left: 5px;"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
        ForeColor="#FF3300"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="pnlContents" runat="server">
                    <table>
                        <tbody>
                            <tr>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <td align="center">
                                            <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                Font-Names="book antiqua" ShowHeader="false" togeneratecolumns="true" AllowPaging="true"
                                                PageSize="50" OnSelectedIndexChanged="gridview1_onselectedindexchanged" OnPageIndexChanging="gridview1_onpageindexchanged"
                                                Width="980px">
                                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                                            </asp:GridView>
                                            <asp:Label ID="lblErr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                                                Font-Names="Book Antiqua"></asp:Label>
                                            <asp:GridView ID="gvStudTest" runat="server" Visible="false" Font-Names="Book Antiqua" 
                                                Font-Size="Medium" ShowHeader="false" OnRowDataBound="gvStudTest_rowbound">
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                            <br />
                                            <br />
                                        </td>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvTestPerfm" runat="server" Visible="false" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnRowDataBound="gvTestPerfm_rowbound">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Chart ID="PerformanceChart" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                                        EnableViewState="true" Font-Size="Medium">
                                        <Series>
                                        </Series>
                                        <Legends>
                                            <asp:Legend Title="Performance Graph" ShadowOffset="3" Font="Book Antiqua">
                                            </asp:Legend>
                                        </Legends>
                                        <Titles>
                                            <asp:Title Docking="Bottom">
                                            </asp:Title>
                                            <asp:Title Docking="Left">
                                            </asp:Title>
                                        </Titles>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                                <AxisY LineColor="White">
                                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                    <MajorGrid LineColor="#e6e6e6" />
                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                </AxisY>
                                                <AxisX LineColor="White">
                                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                    <MajorGrid LineColor="#e6e6e6" />
                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                </AxisX>
                                                <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvAvgcount" runat="server" Visible="false" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnRowDataBound="gvAvgcount_rowbound">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Chart ID="chartAvg" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                                        EnableViewState="true" Font-Size="Medium">
                                        <Series>
                                        </Series>
                                        <Legends>
                                            <asp:Legend Title="Subject Average Chart" ShadowOffset="3" Font="Book Antiqua">
                                            </asp:Legend>
                                        </Legends>
                                        <Titles>
                                            <asp:Title Docking="Bottom">
                                            </asp:Title>
                                            <asp:Title Docking="Left">
                                            </asp:Title>
                                        </Titles>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                                <AxisY LineColor="White">
                                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                    <MajorGrid LineColor="#e6e6e6" />
                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                </AxisY>
                                                <AxisX LineColor="White">
                                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                    <MajorGrid LineColor="#e6e6e6" />
                                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                                </AxisX>
                                                <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div id="rptprint1" runat="server" visible="false">
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                    Height="35px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExcel1" />
                            <asp:PostBackTrigger ControlID="btnprintmaster1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </center>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
