<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAM_Report.aspx.cs" Inherits="CAM_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .mode
        {
            writing-mode: "tb-rl";
        }
        .style11
        {
            width: 68px;
            height: 2px;
        }
        .style14
        {
            height: 2px;
            width: 73px;
        }
        .style33
        {
            height: 2px;
            width: 65px;
        }
        .style34
        {
            height: 2px;
        }
        .style35
        {
            height: 2px;
            width: 138px;
        }
        .style36
        {
            height: 2px;
            width: 54px;
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
        .style37
        {
            top: 219px;
            left: 4px;
            position: absolute;
            height: 21px;
            width: 174px;
        }
        .style38
        {
            top: 221px;
            left: 176px;
            position: absolute;
            height: 21px;
            width: 171px;
        }
        .style39
        {
            top: 270px;
            left: 792px;
            position: absolute;
            height: 21px;
            width: 35px;
        }
        .style40
        {
            top: 270px;
            left: 831px;
            position: absolute;
            height: 27px;
            width: 44px;
        }
        .style41
        {
            top: 214px;
            left: 429px;
            position: absolute;
            height: 33px;
            width: 54px;
        }
        .style42
        {
            margin-left: -368px;
            margin-top: 0;
            position: absolute;
            width: 168px;
            height: 23px;
        }
        .style43
        {
            margin-left: -852px;
            margin-top: 12px;
            position: absolute;
            height: 21px;
            width: 88px;
        }
        .style44
        {
            top: 210px;
            left: 630px;
            position: absolute;
            height: 25px;
            width: 159px;
            right: 185px;
        }
        .style45
        {
            top: 137px;
            left: 571px;
            position: absolute;
        }
        .style46
        {
            top: 119px;
            left: 846px;
            position: absolute;
            width: 216px;
            height: 12px;
            right: -89px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorecc').innerHTML = "";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label1" runat="server" Text="CAM R8-Continuous Assessment Report"
                class="fontstyleheader" ForeColor="Green"></asp:Label>
        </center>
        <br />
        <div>
            <center>
                <table class="maintablestyle" style="margin-left: 0px; height: 73px; width: 1017px;
                    margin-bottom: 0px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True" Style="">
                            </asp:DropDownList>
                        </td>
                        <td class="style35">
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 18px; width: 44px"></asp:Label>
                        </td>
                        <td class="style34">
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="21px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                Style="" Width="71px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style33">
                            <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 56px">
                            </asp:Label>
                        </td>
                        <td class="style34">
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Style="" Width="93px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style33">
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 56px"></asp:Label>
                        </td>
                        <td class="style34">
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style="width: 288px;"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style34">
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                        </td>
                        <td class="style34">
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 48px;" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style34">
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 26px"></asp:Label>
                        </td>
                        <td class="style36">
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Style="width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style11">
                        </td>
                        <td>
                            <asp:Button ID="btnPrintMaster" runat="server" Font-Bold="True" Text="Print Master Setting"
                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44"
                                OnClick="btnPrintMaster_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style34">
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=" Test" Style="width: 31px">
                            </asp:Label>
                        </td>
                        <td class="style14">
                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 21px; width: 88px">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="80px" Style="height: 17px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblttype" runat="server" Text="Type" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Visible="true" Style=""></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlttype" runat="server" AutoPostBack="true" Style="height: 23px;
                                width: 127px;" Font-Bold="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                                <asp:ListItem Text="Mark" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Grade" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        
                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;"
                                Text="Go" Width="36px" Height="26px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" />
                                
                        </td>
                        <td>
                            <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="style39"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                CssClass="style40">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages"
                                OnCheckedChanged="RadioHeader_CheckedChanged" CssClass="style37" />
                            <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page"
                                OnCheckedChanged="Radiowithoutheader_CheckedChanged" CssClass="style38" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="false" CssClass="style41" />
                        </td>
                    </tr>
                </table>
                <table style="height: 0px; width: 272px; margin-left: 0px; margin-top: 0px;">
                    <tr>
                        <td style="margin-top: 0px;">
                            <fieldset style="width: 240px; height: 44px; margin-top: 0px; top: 96px; left: 826px;
                                position: absolute; visibility: hidden;">
                                <legend style="margin-left: 0px; margin-top: 0px;">Criteria For Mark</legend>
                                <br />
                            </fieldset>
                            <asp:RadioButtonList ID="RadioButtonList3" runat="server" CellSpacing="0" OnSelectedIndexChanged="RadioButtonList3_SelectedIndexChanged"
                                AutoPostBack="true" RepeatDirection="Horizontal" Visible="false" Style="margin-left: 1px;
                                margin-top: 0px; margin-bottom: 2px;" Font-Bold="True" CssClass="style46">
                                <asp:ListItem Value="1">Pass</asp:ListItem>
                                <asp:ListItem Value="2">Fail</asp:ListItem>
                                <asp:ListItem Value="3">Absent</asp:ListItem>
                                <asp:ListItem Value="4">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <center>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
                <asp:Label ID="lblgradeerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Style="top: 270px; left: 41px; position: absolute;
                    height: 21px; width: 329px" Text="" Visible="False"></asp:Label>
                &nbsp;
                <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                    Style="top: 270px; left: 4px; position: absolute; height: 19px; width: 168px"></asp:Label>
                &nbsp;&nbsp;
                <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                    Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 270px; left: 180px;
                    position: absolute; height: 21px; width: 126px"></asp:Label>
                &nbsp;&nbsp;
                <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                    Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                    Style="top: 270px; left: 312px; position: absolute; height: 22px; width: 55px;">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                    AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 270px; left: 374px;
                    position: absolute"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                    FilterType="Numbers" />
                &nbsp;&nbsp;
                <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                    Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 270px;
                    left: 412px; position: absolute; height: 21px"></asp:Label>
                &nbsp;&nbsp;
                <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                    OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="17px" Style="top: 270px; left: 507px; position: absolute;
                    width: 34px;"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                    FilterType="Numbers" />
                &nbsp;&nbsp;
                <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 270px; left: 553px;
                    position: absolute; height: 21px; width: 303px"></asp:Label>
            </center>
            <br />
            <center>
                         <div id="printdiv" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spCollegeName" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spAddr" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spReportName" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSem" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="spProgremme" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSection" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
      <center>
                
                
                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#0CA6CA" BorderColor="Black" >
                </asp:GridView>

            </center>
            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        
                    </td>
                    <td style="text-align: right">
                        
                    </td>
                </tr>
            </table>
        </div>
                           
                    
                </center>
           
            <br />
            <center>
                <asp:Label ID="lblnorecc" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>

                <button id="btndirectprint" runat="server" Visible="False"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </center>
            <br />
            <asp:Panel ID="Panel5" runat="server" Width="1100px" Height="600px" ScrollBars="Auto"
                BorderColor="Black" BorderStyle="Double" Style="display: none; height: 400; width: 700;">
                <center>
                    
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button3" runat="server" Text="Close" />
                <br />
            </asp:Panel>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnPrint"
                CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
                Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel4" runat="server" Width="770px" Height="600px" ScrollBars="Auto "
                HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never" BorderColor="Black"
                BorderStyle="Double" Style="display: none; height: 600; width: 800;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book Antiqua;
                        font-size: xx-large; font-weight: bold">
                    </div>
                    <div class="PopupBody">
                    </div>
                    <div class="Controls">
                        <center>
                            
                        </center>
                    </div>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="Close" />
                    <br />
            </asp:Panel>
            <br />
        </div>
        <div>
        </div>
    </body>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnGo" />
            
            
        </Triggers>
    </asp:UpdatePanel>
    
    </html>
</asp:Content>
