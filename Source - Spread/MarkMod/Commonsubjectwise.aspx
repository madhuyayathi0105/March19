<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="Commonsubjectwise.aspx.cs" Inherits="Commonsubjectwise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<style type="text/css">
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
        top: 212px;
        left: 4px;
        position: absolute;
        height: 21px;
        width: 174px;
    }
    .style38
    {
        top: 211px;
        left: 176px;
        position: absolute;
        height: 21px;
        width: 171px;
    }
    .style39
    {
        top: 250px;
        left: 208px;
        position: absolute;
        height: 21px;
        width: 35px;
    }
    .style40
    {
        top: 250px;
        left: 252px;
        position: absolute;
        height: 27px;
        width: 44px;
    }
    .style41
    {
        top: 161px;
        left: 10px;
        position: absolute;
        height: 33px;
        width: 172px;
    }
    .style42
    {
        top: 200px;
        position: absolute;
        width: 34px;
        height: 25px;
    }
    .style43
    {
        top: 250px;
        left: 20px;
        position: absolute;
        height: 19px;
        width: 168px;
    }
    .style44
    {
        top: 251px;
        left: 310px;
        position: absolute;
        height: 21px;
        width: 126px;
    }
    .style45
    {
        top: 250px;
        left: 449px;
        position: absolute;
        height: 22px;
        width: 55px;
    }
    .style46
    {
        top: 250px;
        left: 516px;
        position: absolute;
    }
    .style47
    {
        top: 250px;
        left: 570px;
        position: absolute;
        height: 21px;
    }
    .style48
    {
        top: 250px;
        left: 672px;
        position: absolute;
        width: 34px;
    }
    .style49
    {
        top: 228px;
        left: 553px;
        position: absolute;
        height: 21px;
        width: 303px;
    }
    .style50
    {
        top: 283px;
        left: 20px;
        position: absolute;
        height: 21px;
        width: 329px;
    }
    .style51
    {
        top: 230px;
        left: -4px;
        position: absolute;
        width: 1169px;
    }
    .style52
    {
        height: 73px;
        width: 1017px;
    }
    .style53
    {
        width: 10px;
    }
    .style54
    {
        width: 179px;
        height: 21px;
        position: absolute;
        left: 790px;
        top: 204px;
    }
    .style55
    {
        margin-left: -341px;
        margin-top: -39px;
        position: absolute;
        height: 21px;
        width: 76px;
    }
</style>
<head id="Head1">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</head>
<body>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
   <center>
   <asp:Label ID="lblhead" runat="server" Text=" CR36 - Consolidated Subject Wise
                Report"  Font-Names="Book Antiqua" Font-Bold="true" ForeColor="Green" Font-Size="Large"  />
     </center><br />
    <center>
        <table style="width:1050px; height:100px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="Updp_Batchyear" Visible="true" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Width="100px" Height="30px" CssClass="textbox textbox1 ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Batchyear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Degree" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server" Text="Degree"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="Updp_Degree" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel_Degree" runat="server" CssClass="multxtpanel" Height="200px"
                                    Width="200px">
                                    <asp:CheckBox ID="cb_degree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_degree_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_degree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="Panel_Degree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_dpt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" ReadOnly="true" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Width=" 114px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel_dpt" runat="server" CssClass="multxtpanel " Height="200px" Width="200px">
                                    <asp:CheckBox ID="cb_branch" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_branch_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_branch" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="Panel_dpt" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_sem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Semester"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" Visible="true" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_semester" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                    runat="server" Width="50px" Height="30px" CssClass="textbox textbox1 ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_semester_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            runat="server" Text="Sec"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sec" ReadOnly="true" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                    Width="85px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                <asp:Panel ID="pnl_sec" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                    <asp:CheckBox ID="cb_Sec" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Sec_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_sec" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sec"
                                    PopupControlID="pnl_sec" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_test" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Test"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="Txt_Test" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                    Width="85px" runat="server" Font-Size="Medium" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
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
                    <td>
                        <asp:Label ID="lbl_subject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            runat="server" Text="Subject"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" Visible="true" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_subject" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Width="150px" Height="30px" CssClass="textbox textbox1 ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_subject_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Criteria" Font-Bold="True" Font-Names="Book Antiqua" runat="server"
                            Text="Criteria"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="TextBox1" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Width=" 118px" ReadOnly="true" runat="server" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                                <asp:Panel ID="pnlCustomers" runat="server" CssClass="multxtpanel" Height="450px"
                                    Width="200px">
                                    <asp:CheckBox ID="cb_Criteria" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Criteria_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_Criteria" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Criteria_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Total No of Students</asp:ListItem>
                                        <asp:ListItem Value="1">No.of Students Present</asp:ListItem>
                                        <asp:ListItem Value="2">No.Of Students Absent</asp:ListItem>
                                        <asp:ListItem Value="3">No.Of Students Passed</asp:ListItem>
                                        <asp:ListItem Value="4">No.Of Students Failed</asp:ListItem>
                                        <asp:ListItem Value="5">Pass Percentage</asp:ListItem>
                                        <asp:ListItem Value="6">Total mark</asp:ListItem>
                                        <asp:ListItem Value="7">Subject average</asp:ListItem>
                                        <asp:ListItem Value="8">Max. Mark</asp:ListItem>
                                        <asp:ListItem Value="9">Min Mark</asp:ListItem>
                                        <asp:ListItem Value="10">No.Of Students (S GRADE) >90%</asp:ListItem>
                                        <asp:ListItem Value="11">No.Of Students >80%-89%</asp:ListItem>
                                        <asp:ListItem Value="12">No.Of Students >70%-79%</asp:ListItem>
                                        <asp:ListItem Value="13">No.Of Students >60%-69%</asp:ListItem>
                                        <asp:ListItem Value="14">No.Of Students >50%-59%</asp:ListItem>
                                        <asp:ListItem Value="15">No.Of Students <49%</asp:ListItem>
                                        <asp:ListItem Value="16">No.Of Students <45% (University Cutoff)</asp:ListItem>
                                        <asp:ListItem Value="17">Pass PercentageChart</asp:ListItem>
                                        <asp:ListItem Value="18">No.Of Student (S GRADE) >90% Chart</asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="TextBox1"
                                    PopupControlID="pnlCustomers" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_optional" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            runat="server" Text="Optional Min Pass Mark"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtoptiminpassmark" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Width=" 70px" MaxLength="3" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtoptiminpassmark"
                            FilterType="Numbers,custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Width="59px" CssClass="textbox btn2" Text="Go" OnClick="btn_go_Click" />

                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        
    </center>
    <br />
  
    <asp:Label ID="errmsg" runat="server" Text="" ForeColor="Red" Visible="False" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    
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
            <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA" BorderColor="Black"  Width="950px" >
                                    </asp:GridView>

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
    <center>
        <br />
        <div>
            <center>
                <asp:GridView ID="chart_passpercentage" runat="server" Visible="false" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:GridView>
                <asp:Chart ID="ChartPassPercent" runat="server" Height="500px" Width="800px" Visible="false"
                    Font-Names="Book Antiqua" EnableViewState="true" Font-Size="Medium">
                    <Series>
                    </Series>
                    <Legends>
                        <asp:Legend Title="Pass Percentage" ShadowOffset="2" Font="Book Antiqua">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Docking="Top" Text="PASS PERCENTAGE" Font="Microsoft Sans Serif, 12pt">
                        </asp:Title>
                        <asp:Title Docking="Bottom" Text="Marks">
                        </asp:Title>
                        <asp:Title Docking="Left" Text="PASS %">
                        </asp:Title>
                    </Titles>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                            <AxisY LineColor="White" Maximum="100">
                                <LabelStyle Font="Book Antiqua, 8.25pt" />
                                <MajorGrid LineColor="#e6e6e6" />
                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                            </AxisY>
                            <AxisX LineColor="White">
                                <LabelStyle Font="Book Antiqua, 8.25pt" />
                                <MajorGrid LineColor="#e6e6e6" />
                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                            </AxisX>
                            <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </center>
        </div>
        <%--  <div id="chart_passpercentage" runat="server" visible="false" >
        <asp:Chart ID="ChartPassPercent" runat="server" Height="500px" Width="1000px" Visible="true"
            Font-Names="Book Antiqua"  Font-Bold="True" EnableViewState="true" Style="overflow: auto;" Font-Size="Medium">
            <Series>
            </Series>
            <Legends>
                <asp:Legend Title="Pass Percentage" ShadowOffset="2" Font="Book Antiqua">
                </asp:Legend>
            </Legends>
            <Titles>
              
                <asp:Title Docking="Top"   Text="PASS PERCENTAGE" Font="Microsoft Sans Serif, 12pt">
                </asp:Title>
            </Titles>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                    <AxisY LineColor="White" Maximum="100" Title="Pass Percentage">
                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                        <MajorGrid LineColor="#e6e6e6" />
                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                    </AxisY>
                    <AxisX LineColor="White" IsLabelAutoFit="true" Title="Name Of The Tests">
                        <LabelStyle Font="Trebuchet MS, 8.25pt" Angle="-90" Interval="1" />
                        <MajorGrid LineColor="#e6e6e6" />
                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                    </AxisX>
                   </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
     
    </div>--%>
    </center>
    <br />
    <center>
        <div id="S_grads" runat="server" visible="false">
            <center>
                <asp:Chart ID="S_GRADE" runat="server" Height="500px" Width="800px" Visible="false"
                    Font-Names="Book Antiqua" EnableViewState="true" Style="overflow: auto;" Font-Size="Medium">
                    <Series>
                    </Series>
                    <Legends>
                        <asp:Legend Title="No.of students S GRADE" ShadowOffset="2" Font="Book Antiqua">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Docking="Bottom" Text="No.of students (S GRADE) >90%" Font="Book Antiqua, 12pt">
                        </asp:Title>
                        <asp:Title Docking="Left" Text="Axis Title" Font="Book Antiqua, 12pt">
                        </asp:Title>
                        <asp:Title Docking="Top" Text="No.of students (S GRADE) >90%" Font="Book Antiqua, 12pt">
                        </asp:Title>
                    </Titles>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                            <AxisY LineColor="White">
                                <LabelStyle Font="Book Antiqua, 8.25pt" />
                                <MajorGrid LineColor="#e6e6e6" />
                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                            </AxisY>
                            <AxisX LineColor="White" IsLabelAutoFit="true">
                                <LabelStyle Font="Book Antiqua, 8.25pt" Angle="-90" Interval="1" />
                                <MajorGrid LineColor="#e6e6e6" />
                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                            </AxisX>
                            <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />
                <div style="margin-top: -2px; margin-left: -5px;">
                    <br />
                    <asp:GridView ID="GridViewchart" runat="server" Width="645px" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:GridView>
                    <br />
                </div>
            </center>
        </div>
    </center>
    <br />
    <center>
        <div id="rptprint1" runat="server" visible="false">
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
             <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

            <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
        </div>
        <br />
        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
    </center>
    </form>

    </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnExcel1" />
                                </Triggers>
                             </asp:UpdatePanel>

                             <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</body>
</html>
</asp:Content>

