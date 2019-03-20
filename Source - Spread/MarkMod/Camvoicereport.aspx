<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Camvoicereport.aspx.cs" Inherits="Camvoicereport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
            left: 747px;
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
            top: 200px;
            left: 4px;
            position: absolute;
            height: 21px;
            width: 76px;
            right: 716px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerror').innerHTML = "";

        }


        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= gridviewload.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[1];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
                Style="width: 1205px">
                <center>
                    <asp:Label ID="Label1" runat="server" Text="CAM-15 Voice Call Send" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
                </center>
            </asp:Panel>
        </div>
        <br />
        <div>
            <asp:Panel ID="Panel1" runat="server" BackColor="LightBlue" BorderColor="Black" BorderStyle="Solid"
                ClientIDMode="Static" Width="1205px" Style="border-width: 1px; height: 70px;">
                <table style="margin-left: 0px; margin-bottom: 0px;" class="style52">
                    <tr>
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
                        <td colspan="2">
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                            <%-- </td>
                        <td class="style34">--%>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 48px;" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            <%-- </td>
                        <td class="style34">--%>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="height: 21px; width: 26px"></asp:Label>
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Style="width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style36">
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
                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                Style="width: 168px; height: 23px;" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="84px" Style="height: 17px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 21px; width: 58px">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="80px" Style="height: 17px; right: 464px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                            <asp:CheckBox ID="cbsms" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                Text="SMS" Style="" />
                            <asp:Panel ID="panel3" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                Width="150px" Height="27px" Style="margin-left: 150px; margin-top: -25px;">
                                <asp:RadioButtonList ID="rblsmstype" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblsmstype_Selected">
                                </asp:RadioButtonList>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbvoice" runat="server" Text="Voice" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="width: 40px;" />
                            <asp:Panel ID="panelfilter" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="248px" Height="28px" Style="margin-left: 68px; margin-top: -24px;">
                                <asp:CheckBox ID="cbstudent" runat="server" Font-Bold="true" Font-Size="Medium" Checked="true"
                                    Font-Names="Book Antiqua" Text="Student" />
                                <asp:CheckBox ID="cbfather" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Text="Father" />
                                <asp:CheckBox ID="cbmother" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Text="Mother" />
                            </asp:Panel>
                        </td>
                        <td>
                        </td>
                        <td class="style53">
                            <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages"
                                CssClass="style37" />
                            <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page"
                                CssClass="style38" />
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lbltesterr" runat="server" Font-Bold="True" Font-Size="Medium" Visible="false"
                                Font-Names="Book Antiqua" ForeColor="Red" CssClass="style54">Please Select The Test</asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center; margin-left:-19px;"
                                Text="Go" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"  />
                        </td>
                        <td>
                        <asp:CheckBox ID="cbwithresult" runat="server" Text="With Result" Width="150px" Font-Bold="true" style="margin-left:-7px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>


            <%-- Hidden by srinath 12/3/2014  <asp:Image ID="Image1" runat="server" Style="top: 373px; left: 229px; position: absolute;
                height: 16px; width: 14px" />--%>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"
                            CssClass="style50"></asp:Label>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            CssClass="style43">
                        </asp:Label>
                        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44"></asp:Label>
                        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style45">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                            AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style46"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                            FilterType="Numbers" />
                        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style47"></asp:Label>
                        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="17px" CssClass="style48"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                            FilterType="Numbers" />
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style49"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridviewload" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="70"
                            OnPageIndexChanging="gridviewload_onpageindexchanged" OnRowDataBound="gridviewload_RowDataBound" BackColor="AliceBlue" >
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label runat="server" ID="lblsno" Text='<%#Container.DisplayIndex+1 %>' Width="20px" /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="lblSelectAll" Text="Select All"  />
                                        <asp:CheckBox ID="allchk" runat="server"  onchange="return SelLedgers();" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectchk" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        <asp:Button ID="btn" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Visible="false"
                            OnClick="btn_Click" Font-Size="Medium" Text="Send" />
                        <asp:Button ID="Button1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Visible="false" Style="opacity: 0;" Font-Size="Medium" Text="Send" />
                    </td>
                </tr>
            </table>
        </div>
        <%-- </form>--%>
    </body>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalertmsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnpopupalert" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                            OnClick="btnpopupalert_Click" Text="Ok" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    </center>
</asp:Content>
