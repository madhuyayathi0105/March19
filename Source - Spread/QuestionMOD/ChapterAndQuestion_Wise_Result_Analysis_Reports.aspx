<%@ Page Title="Chapter And Question Wise Result Analysis Report" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ChapterAndQuestion_Wise_Result_Analysis_Reports.aspx.cs"
    Inherits="ChapterAndQuestion_Wise_Result_Analysis_Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var chart1 = document.getElementById("<%=ChartQuesWiseDmg.ClientID %>");
            var chart2 = document.getElementById("<%=ChartChapterWiseDmg.ClientID %>");
            var panel = document.getElementById("<%=divprint.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
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
    <style tyle="text/css">
        body
        {
            font-family: Book Antiqua;
            height: auto;
            background-color: #ffffff;
            color: Black;
        }
        .Chartdiv
        {
            background-color: #ffffff;
            margin: 0px;
            color: #000000;
            position: relative;
            font-family: Book Antiqua;
            height: auto;
            width: 100%;
        }
        .Header
        {
            font-weight: bold;
            text-align: center;
            font-size: 22px;
            color: Green;
            margin-top: 20px;
            margin-bottom: 20px;
            line-height: 3em;
        }
        .fontCommon
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: #000000;
        }
        .defaultHeight
        {
            width: auto;
            height: auto;
        }
        #divSpread2
        {
            display: none;
        }
        .printclass
        {
            display: none;
        }
        #ChartQuesWiseDmg
        {
            src: url('~\QueswiseDmg.png');
        }
        #ChartChapterWiseDmg
        {
            src: url('~\ChapterWiseDmg.png');
        }
        @media print
        {
            #divprint
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
            #divSpread2
            {
                display: block;
            }
            #FpSpread2, FpSpread2_viewport
            {
                display: block;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 100%; height: auto;">
            <table>
                <thead>
                    <tr>
                        <td colspan="3">
                            <center>
                                <span class="Header">Chapter And Question Wise Result Analysis Report</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <fieldset id="maindiv" runat="server" style="width: 960px; margin-left: 0px; height: auto;
                    border-color: silver; border-radius: 10px;">
                    <center>
                        <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;">
                            <table style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                                box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px;
                                padding: 1em; margin-left: 0px; width: 930px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="50px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlsubject_Selectchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTest" runat="server" Text="Test" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTest" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlTest_Selectchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="display: none;">
                                                    <asp:Label ID="lblQuestions" runat="server" Text="Questions" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td style="display: none;">
                                                    <asp:UpdatePanel ID="UpnlQuestions" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_Questions" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="Panel_Questions" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="250px">
                                                                <asp:CheckBox ID="cb_Questions" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Questions_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_Questions" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Questions_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_Questions"
                                                                PopupControlID="Panel_Questions" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Width="59px" CssClass="textbox defaultHeight" Text="Go" OnClick="btngo_Click" />
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkShowSelQuestions" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" Text="Show Questions" Checked="false" AutoPostBack="true"
                                                        OnCheckedChanged="chkShowSelQuestions_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <center>
                        <div id="divSpread" runat="server">
                            <FarPoint:FpSpread ID="FpSpread1" AutoPostBack="false" Width="1050px" runat="server"
                                Visible="true" BorderStyle="Solid" BorderWidth="1px" CssClass="spreadborder"
                                ShowHeaderSelection="false" Style="width: 100%; height: auto; display: block;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <br />
                            <br />
                            <br />
                        </div>
                        <%--<asp:Button ID="btnSave" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
            Width="59px" CssClass="textbox btn2" Text="Save" OnClick="btnSave_Click" />--%>
                    </center>
                    <br />
                    <br />
                    <div id="divprint" runat="server">
                        <div id="divSpread2" runat="server" class="printclass">
                            <%-- <div id="Header" runat="server">--%>
                            <center>
                                <asp:Label ID="spancollname" runat="server" class="printclass" Style="text-align: center;
                                    font-weight: bolder;">
                                </asp:Label>
                                <br />
                                <asp:Label ID="spanaddr" runat="server" class="printclass" Style="text-align: center;
                                    font-weight: bolder;">
                                </asp:Label>
                                <br />
                                <asp:Label ID="spandegdetails" runat="server" class="printclass" Style="text-align: center;
                                    font-weight: bolder;">
                                </asp:Label>
                                <br />
                                <asp:Label ID="spanTitle" runat="server" class="printclass" Style="text-align: center;
                                    font-weight: bolder;">
        Chapter And Question Wise Result Analysis Report
                                </asp:Label>
                            </center>
                            <br />
                            <asp:Label ID="spanSub" runat="server" class="printclass" Style="text-align: left;
                                font-weight: bolder;">
                            </asp:Label>
                            <br />
                            <%--  </div>--%>
                            <FarPoint:FpSpread ID="FpSpread2" AutoPostBack="true" Width="1050px" runat="server"
                                Visible="true" BorderStyle="Solid" BorderWidth="0px" ShowHeaderSelection="false"
                                Style="width: 100%; height: auto; display: block; padding: 10px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <br />
                            <br />
                        </div>
                        <asp:Label ID="spanQues" runat="server" class="printclass" Style="text-align: left;
                            font-weight: bolder; font-size: 18px;">
       Question Wise Result Analysis Chart
                        </asp:Label>
                        <br />
                        <br />
                        <center>
                            <asp:GridView ID="gvQuesWiseDmg" runat="server" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnRowDataBound="gvQuesWiseDmg_rowbound">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </center>
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Chart ID="ChartQuesWiseDmg" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                                EnableViewState="true" Font-Size="Medium">
                                <Series>
                                </Series>
                                <Legends>
                                    <asp:Legend Title="Question Wise Damage" ShadowOffset="3" Font="Book Antiqua">
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
                        </center>
                        <br />
                        <br />
                        <asp:Label ID="span1" runat="server" class="printclass" Style="text-align: left;
                            font-weight: bolder; font-size: 18px;">
        Chapter Wise Result Analysis Chart
                        </asp:Label>
                        <br />
                        <br />
                        <center>
                            <asp:GridView ID="gvChapWiseDmg" runat="server" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnRowDataBound="gvChapWiseDmg_rowbound">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </center>
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Chart ID="ChartChapterWiseDmg" runat="server" Width="800px" Visible="false"
                                Font-Names="Book Antiqua" EnableViewState="true" Font-Size="Medium" RenderType="ImageTag"
                                ImageType="Jpeg">
                                <Series>
                                </Series>
                                <Legends>
                                    <asp:Legend Title="Class Chapter Wise Damage" ShadowOffset="3" Font="Book Antiqua">
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
                        </center>
                    </div>
                    <center>
                        <div id="rptprint1" class="noprint" runat="server" visible="false">
                            <br />
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
                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        </div>
                    </center>
                </fieldset>
            </center>
            <div id="popupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblpopuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                                Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                                OnClick="btn_errorclose_Click" Text="Ok" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <%--<div id="divShowQuestions" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <asp:ImageButton ID="imgbtnClose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
            Style="height: 30px; width: 30px; position:relative;"
            OnClick="imgbtnClose_OnClick" />
        <center>
            <div id="divGridQuestions" runat="server" class="table" style="background-color: White;
                height: auto; width: 100%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                border-radius: 10px; position: relative;">
                <center>
                    <table style="margin: 0px;">
                        <tr>
                            <td colspan="2">
                                <FarPoint:FpSpread ID="FpShowQuestions" AutoPostBack="false" runat="server" Visible="true"
                                    BorderStyle="Solid" BorderWidth="0px" ShowHeaderSelection="false" OnUpdateCommand="FpShowQuestions_UpdateCommand"
                                    Style="width: 100%; height: auto; display: table; border: 0px solid #000000;"
                                    CssClass="Chartdiv">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnShowQuesGo" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Width="59px" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />                            
                                <asp:Button ID="btnExit" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    CssClass="textbox btn2" Text="Exit" OnClick="btnExit_Click" />
                            </td>
                        </tr>
                    </table>                    
                </center>
            </div>
        </center>
    </div>--%>
            <div id="divShowQuestions" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: auto; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <asp:ImageButton ID="imgbtnClose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: relative; top: 0%; left: 0%" OnClick="imgbtnClose_OnClick" />
                    <div id="divGridQuestions" runat="server" class="table" style="background-color: White;
                        height: auto; width: auto; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        border-radius: 10px; position: relative;">
                        <center>
                            <table style="margin: 0px;">
                                <tr>
                                    <td colspan="2">
                                        <FarPoint:FpSpread ID="FpShowQuestions" AutoPostBack="false" runat="server" Visible="true"
                                            BorderStyle="Solid" BorderWidth="0px" ShowHeaderSelection="false" OnUpdateCommand="FpShowQuestions_UpdateCommand"
                                            Style="width: 100%; height: auto; display: table; border: 0px solid #000000;"
                                            CssClass="Chartdiv">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnShowQuesGo" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Width="59px" CssClass="textbox defaultHeight" Text="Go"
                                            OnClick="btngo_Click" />
                                        <asp:Button ID="btnExit" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox defaultHeight" Text="Exit" OnClick="btnExit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </div>
    </center>
</asp:Content>
