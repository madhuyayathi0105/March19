<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Target_report.aspx.cs" Inherits="Target_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            top: 210px;
            position: absolute;
            width: 168px;
            height: 23px;
            left: 396px;
        }
        .style43
        {
            top: 210px;
            left: 7px;
            position: absolute;
            height: 21px;
            width: 76px;
            right: 891px;
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
            left: 957px;
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
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <body>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green;">CAM R17-Target Report</span>
            </center>
            <br />
        </div>
        <div>
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
                        <br />
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
                        <br />
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
                        <br />
                    </td>
                    <td class="style11">
                    </td>
                    <td>
                        <asp:Button ID="btnPrintMaster" runat="server" Font-Bold="True" Text="Print Master Setting"
                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrintMaster_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="style34">
                        <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text=" Test" Style="width: 31px">
                        </asp:Label>
                    </td>
                    <td class="style14">
                        <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Style="" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="75px" Style="height: 17px;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 21px; width: 90px">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" Style="height: 17px; right: 637px;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rbselectionlist" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" ForeColor="Black"
                            OnSelectedIndexChanged="rbselectionlist_selectedindexchanged" Style="text-align: center;">
                            <asp:ListItem Selected="True" Value="1">Arrear Based</asp:ListItem>
                            <asp:ListItem Value="2">Manual</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Label ID="lblnumberofarreares" runat="server" Text="No of arrears" Font-Bold="True"
                            Style="text-align: center;" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtnofarreares" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                            onkeypress="return allowOnlyNumber(event);" Font-Size="Medium" ForeColor="Black"
                            Style="text-align: center; width: 50px;" MaxLength="2">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;"
                            Text="Go" Width="36px" Height="24px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:Button ID="btnselecttargetstudent" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnselecttargetstudent_Click" Font-Size="Medium" Text="Select" Style="text-align: center;" />
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
                        <fieldset style="width: 240px; height: 44px; visibility: hidden;">
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
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Style="top: 270px; left: 41px; position: absolute;
                height: 21px; width: 329px" Text="No Record(s) Found" Visible="False"></asp:Label>
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
            <%--<br />
         
            <br />
           <br />
            <br />
      
             <br />
           <br />
            <br />--%>
            <center>
                <FarPoint:FpSpread ID="FpEntry" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Width="900px">
                    <%--style="top: 320px; left: 103px; height:167px; position: absolute" Visible="False">--%>
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark" ButtonType="PushButton">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="Black">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </center>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Label ID="lblnorecc" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
            <asp:Panel ID="Panel5" runat="server" Width="1100px" Height="600px" ScrollBars="Auto"
                BorderColor="Black" BorderStyle="Double" Style="display: none; height: 400; width: 700;">
                <center>
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="600" Width="1100" Visible="False" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="Never">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
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
                    <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book 

Antiqua; font-size: xx-large; font-weight: bold">
                    </div>
                    <div class="PopupBody">
                    </div>
                    <div class="Controls">
                        <center>
                            <FarPoint:FpSpread ID="FpSpreadPrint" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="600" Width="770" Visible="true" HorizontalScrollBarPolicy="Never"
                                VerticalScrollBarPolicy="Never">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="True">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
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
            <asp:Panel ID="panelrollnopop" runat="server" BorderColor="Black" BackColor="White"
                Visible="false" BorderWidth="2px" Style="left: 150px; top: 250px; position: absolute;"
                Height="391px" Width="690px">
                <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold">
                    <table style="text-align: left">
                        <tr>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatchadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexXhanged" AutoPostBack="True"
                                    Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegreeadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddldegreeadd_SelectedIndexXhanged"
                                    AutoPostBack="True" Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranchadd" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbrachadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbrachadd_SelectedIndexXhanged"
                                    AutoPostBack="True" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsemadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlsem_SelectedIndexXhanged" AutoPostBack="True"
                                    Width="40px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Labelsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsecadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlsec_SelectedIndexXhanged" AutoPostBack="True"
                                    Width="40px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <FarPoint:FpSpread ID="sprdselectrollno" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Height="300" Width="680" HorizontalScrollBarPolicy="AsNeeded"
                                    VerticalScrollBarPolicy="Never" OnUpdateCommand="sprdselectrollno_UpdateCommand"
                                    OnCellClick="sprdselectrollno_CellClick" OnPreRender="sprdselectrollno_SelectedIndexChanged">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <asp:Label ID="lblnoselecterr" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    ForeColor="Red" Visible="false" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <center>
                                    <asp:Button ID="btnsave" runat="server" Text="Calculate" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnsave_Click" />
                                    <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="70px" OnClick="btnexit_Click" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>
    </body>
    </html>
</asp:Content>
