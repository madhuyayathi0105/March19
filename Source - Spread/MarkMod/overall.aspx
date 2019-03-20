<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="overall.aspx.cs" Inherits="overall" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
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
            top: 226px;
            left: 10px;
            position: absolute;
            height: 21px;
            width: 249px;
        }
    </style>
    <body>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">CR4 - Overall Best Performance</span>
            <br />
            <br />
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
                            Font-Names="Book Antiqua" Style="height: 20px; width: 42px"></asp:Label>
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
                        <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Style="height: 21px; width: 30px"></asp:Label>
                        <asp:LinkButton ID="LinkButton4" runat="server" BackColor="White" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" OnClick="LinkButton4_Click"
                            Style="height: 17px; width: 161px;" Enabled="False" Visible="False">OverAll Best Performance</asp:LinkButton>
                    </td>
                    <td class="style34">
                        <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 48px;" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style34">
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Style="height: 21px; width: 27px"></asp:Label>
                    </td>
                    <td class="style36">
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Style="width: 93px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td class="style11">
                    </td>
                    <td class="style34">
                        <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text=" Test" Style="width: 31px">
                        </asp:Label>
                    </td>
                    <td class="style14">
                        <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                            Height="21px" Style="width: 171px;" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltop" runat="server" Text="Top" Style="height: 24px; width: 57px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttop" runat="server" Style="width: 50px;" Font-Bold="True" Font-Names="Arial"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="markfilter" runat="server" FilterType="Numbers"
                            TargetControlID="txttop">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;"
                            Text="Go" Width="40px" Height="28px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrintMaster" runat="server" Text="Print Master Setting" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrintMaster_Click"
                            Style="width: 160px;" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="There is no record found" Visible="False"></asp:Label>
            <asp:Label ID="lblerror" runat="server" Text="Label" Style="color: Red; font-size: medium;"></asp:Label>
            <br />
        </center>
        <center>
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="600" Width="600" Visible="true" HorizontalScrollBarPolicy="Never"
                VerticalScrollBarPolicy="Never" ShowHeaderSelection="false">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
        <br />
        <center>
            <asp:Label ID="lblerr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Button ID="BtnPrint" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="BtnPrint_Click" Font-Size="Medium" Text="Print" Width="127px" />
        </center>
        <br />
        <center>
            <asp:Button ID="Button2" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="Button2_Click" Style="height: 33px; width: 54px"
                Enabled="False" Visible="False" />
            <br />
            &nbsp;
            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                Style="top: 225px; left: 4px; position: absolute; height: 21px; width: 219px"></asp:Label>
            &nbsp;&nbsp;
            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 226px; left: 230px;
                position: absolute; height: 21px; width: 126px"></asp:Label>
            &nbsp;&nbsp;
            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                Height="24px" Width="58px" Style="top: 223px; left: 365px; position: absolute">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 228px; left: 433px;
                position: absolute"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 224px;
                left: 476px; position: absolute; height: 21px"></asp:Label>
            &nbsp;&nbsp;
            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Height="17px" Style="top: 226px; left: 579px; position: absolute;
                width: 34px;"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 226px; left: 628px;
                position: absolute; height: 21px; width: 303px"></asp:Label>
        </center>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Panel ID="Panel5" runat="server">
            <center>
                <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Height="600" Width="600" HorizontalScrollBarPolicy="Never"
                    VerticalScrollBarPolicy="Never" Visible="false" Style="top: 238px; left: 103px;
                    height: 600px; position: absolute" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonType="PushButton" ShowPDFButton="True" ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
        </asp:Panel>
        <center>
            <FarPoint:FpSpread ID="FpEntry" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="900px" Style="top: 258px; left: 103px; height: 167px;
                position: absolute" Visible="False" ShowHeaderSelection="false">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AllowSort="true">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
    </body>
    </html>
</asp:Content>
