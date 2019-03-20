<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Consolidated_report.aspx.cs" Inherits="Consolidated_report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
            left: 65px;
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
    <body>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
                Style="width: 1061px">
                <center>
                    <asp:Label ID="Label1" runat="server" Text=" Consolidated Attendance And Mark Details"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
                </center>
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="Panel1" runat="server" Height="72px" BackColor="LightBlue" BorderColor="Black"
                BorderStyle="Solid" ClientIDMode="Static" Width="1000px" BorderWidth="1px" Style="">
                <table style="margin-left: 0px; height: 73px; width: 1017px; margin-bottom: 0px;">
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
                    </tr>
                    <tr>
                        <td class="style34">
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=" Test" Style="width: 31px">
                            </asp:Label>
                        </td>
                        <td class="style14">
                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Style="" Font-Bold="True"
                                OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkretest" runat="server" Text="Re-Test" AutoPostBack="true" OnCheckedChanged="Retest_CheckedChanged"
                                Style="width: 100px;" Font-Bold="True" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrintMaster" runat="server" Font-Bold="True" Text="Print Master Setting"
                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44" />
                        </td>
                        <td>
                            <asp:Label ID="lblFromDate" Visible="false" runat="server" Text="From Date" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px" Style="height: 17px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblToDate" Visible="false" runat="server" Text="To Date" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px; width: 58px">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="80px" Style="height: 17px; right: 637px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages"
                                OnCheckedChanged="RadioHeader_CheckedChanged" />
                            <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page"
                                OnCheckedChanged="Radiowithoutheader_CheckedChanged" />
                            <%--</td>
                        <td>--%>
                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;"
                                Text="Go" Width="36px" Height="26px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="false" OnClick="btnPrint_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="margin-top: 0px;" colspan="5">
                            <%--Criteria For Mark--%>
                            <asp:RadioButtonList ID="RadioButtonList3" runat="server" CellSpacing="0" OnSelectedIndexChanged="RadioButtonList3_SelectedIndexChanged"
                                AutoPostBack="true" RepeatDirection="Horizontal" Visible="false" Style="" Font-Bold="True">
                                <asp:ListItem Value="1">Pass</asp:ListItem>
                                <asp:ListItem Value="2">Fail</asp:ListItem>
                                <asp:ListItem Value="3">Absent</asp:ListItem>
                                <asp:ListItem Value="4">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                Style="width: 1030px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
            </asp:Panel>
            </center>
            <%-- <br />
                 <br />
                      <br />--%>
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
            <asp:Panel ID="Panel6" runat="server">
                <asp:Panel ID="newpnl" runat="server" Style="margin-left: 1px;">
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        Font-Names="book antiqua" togeneratecolumns="true" ShowHeader="false" 
                                        OnRowDataBound="OnRowDataBound" Width="980px">
                                    
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                                    </asp:GridView>
                                </center>
                                <%--hai--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                                <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                <asp:Button ID="Button2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Visible="false" Text="Print" OnClick="btnPrint_Click1" Width="127px" />
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblnorecc" runat="server" Font-Bold="True" Width="650px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
            </asp:Panel>
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
        </div>
    </body>
    </html>
</asp:Content>
