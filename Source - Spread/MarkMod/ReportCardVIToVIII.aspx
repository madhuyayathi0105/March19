<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="ReportCardVIToVIII.aspx.cs" Inherits="ReportCardVIToVIII" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <style type="text/css">
                .gvRow
                {
                    margin-right: 0px;
                    margin-top: 325px;
                }
                .gvRow td
                {
                    background-color: #F0FFFF;
                    font-family: Book Antiqua;
                    font-size: medium;
                    padding: 3px;
                    border: 1px solid black;
                }
                
                .gvAltRow td
                {
                    font-family: Book Antiqua;
                    font-size: medium;
                    padding: 3px;
                    border: 1px solid black;
                    background-color: #CFECEC;
                }
            </style>
            <script type="text/javascript">
                function display() {
                    document.getElementById('MainContent_lblnorec').innerHTML = "";
                }
            </script>
            <center>
                <div style="width: 1016px; height: 26px; margin-left: 10px; margin: 50px auto 159px -28px;
                    padding-left: auto; padding-right: auto; background-color: ; text-align: right;">
                    <center>
                                   
                                   
                               
                        <asp:Label ID="lbl" runat="server" Text="Student Grade and Report Card" Font-Bold="true"
                            Font-Names="Bood Antiqua" Font-Size="Large" ></asp:Label>
                                   
                                   
                       <%-- <asp:LinkButton ID="back" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 70px; position: absolute;" PostBackUrl="~/Default_login.aspx" ForeColor="White">Back</asp:LinkButton>
                        
                        <asp:LinkButton ID="home" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 99px; position: absolute;" PostBackUrl="~/Default_login.aspx" ForeColor="White">Home</asp:LinkButton>
                        
                        <asp:LinkButton ID="log" runat="server" OnClick="go_Click" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 136px; position: absolute;" ForeColor="White">Logout</asp:LinkButton>--%>
                    </center>
                </div>
            </center>
            <div style="width: 996px; height: 66px; background-color: -webkit-border-radius: 10px;
                -moz-border-radius: 10px; padding: 10px; padding-left: auto; padding-right: auto;
                margin: -159px  auto -159px 0px; background-color: #219DA5;">
                <center>
                    <table style="margin-left: -101px; margin-top: -220px; position: absolute; height: 50px;
                        width: 600px; margin-bottom: 0px; line-height: 27px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblschool" runat="server" Width="46px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 99px; top: 225px;"
                                    Font-Size="Medium" Text="School" ForeColor="#ffffff"></asp:Label>
                                <asp:DropDownList ID="ddschool" runat="server" Width="213px" Height="25px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 158px; top: 225px;"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddschool_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblyear" runat="server" Width="50px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 385px; top: 225px;"
                                    Font-Size="Medium" Text="Year" ForeColor="#ffffff"></asp:Label>
                                <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                                    OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                                    Style="position: absolute; left: 428px; top: 225px;" Font-Size="Medium" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblschooltype" runat="server" Width="125px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 496px; top: 225px;"
                                    Font-Size="Medium" Text="School Type" ForeColor="#ffffff"></asp:Label>
                                <asp:DropDownList ID="ddschooltype" runat="server" Width="80px" Height="25px" AutoPostBack="true"
                                    OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 595px; top: 225px;"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblstandard" runat="server" Width="37px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 684px; top: 225px;"
                                    Font-Size="Medium" Text="Standard" ForeColor="#ffffff"></asp:Label>
                                <asp:DropDownList ID="ddstandard" runat="server" Width="110px" Height="25px" AutoPostBack="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged"
                                    Style="position: absolute; left: 759px; top: 225px;" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblterm" runat="server" Font-Color="white" Width="100px" Height="20px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute; left: 879px;
                                    top: 225px;" Font-Size="Medium" Text="Term" ForeColor="#ffffff"></asp:Label>
                                <asp:DropDownList ID="dropterm" runat="server" Width="35px" Height="25px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 922px; top: 225px;"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Iblsec" runat="server" Style="position: absolute; left: 965px; width: 40px;
                                    top: 225px; color: white;" Font-Size="Medium" Font-Bold="true" Text="Sec"></asp:Label>
                                <asp:DropDownList ID="dropsec" runat="server" Width="44px" Height="25px" Style="position: absolute;
                                    left: 994px; top: 225px;" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="dropsec_OnSelectedIndexChanged"
                                    Font-Bold="true" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="display: none;">
                                <asp:Label ID="lblrpt" runat="server" Style="position: absolute; left: 99px; width: 40px;
                                    top: 260px; color: white;" Font-Size="Medium" Font-Bold="true" Text="Report"></asp:Label>
                                <asp:DropDownList ID="ddlreporttype" runat="server" Width="213px" Height="25px" Style="position: absolute;
                                    left: 158px; top: 260px;" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddlreporttype_OnSelectedIndexChanged"
                                    Font-Bold="true" Font-Size="Medium" Visible="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text=" Test" Style="font-family: Book Antiqua; font-size: medium;
                                    color: White; font-weight: bold; top: 260px; position: absolute; left: 386px;
                                    width: 99px;">
                                </asp:Label>
                                <asp:UpdatePanel ID="udpnlTest" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_Test" runat="server" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                            Height="30px" Style="font-size: medium; font-weight: bold; position: absolute;
                                            height: 20px; font-family: 'Book Antiqua'; top: 260px; left: 424px;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="100px">---Select---</asp:TextBox>
                                        <asp:Panel ID="ptest" runat="server" CssClass="MultipleSelectionDDL" Style="width: 140px;">
                                            <asp:CheckBox ID="chktest" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chktest_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="true" />
                                            <asp:CheckBoxList ID="chklstest" runat="server" Font-Size="Medium" Width="130px"
                                                Height="58px" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true"
                                                OnSelectedIndexChanged="chklsttest_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_Test"
                                            PopupControlID="ptest" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="Header" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; color: White;
                                    font-weight: bold; top: 260px; position: absolute; left: 99px; width: 118px;"></asp:Label>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtaccheader" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Width="150px" Style="font-size: medium; font-weight: bold; position: absolute;
                                            height: 20px; font-family: 'Book Antiqua'; top: 260px; left: 158px;">---Select---</asp:TextBox>
                                       <asp:Panel ID="paccheader" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                            <asp:CheckBox ID="chkaccheader" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkaccheader_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                                                HoverNodeStyle-BackColor="Black" Height="258px" Width="343px" Font-Names="Book Antiqua"
                                                ForeColor="Black" ShowCheckBoxes="All">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtaccheader"
                                            PopupControlID="paccheader" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Visible="false" Style="font-family: Book Antiqua; font-size: medium;
                                    color: White; font-weight: bold; top: 260px; position: absolute; left: 386px;
                                    width: 100px;"></asp:Label>
                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="54px" CausesValidation="True"
                                    Visible="false" Style="position: absolute; left: 490px; top: 260px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Visible="false" Style="font-family: Book Antiqua; font-size: medium;
                                    color: White; font-weight: bold; top: 260px; position: absolute; left: 558px;
                                    width: 100px;"></asp:Label>
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="54px" CausesValidation="True"
                                    Visible="false" Style="position: absolute; left: 640px; top: 260px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Button ID="btngo" runat="server" Style="background-color: silver; border: 2px solid white;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            height: 27px; margin-left: -261px; margin-top: 40px; position: absolute; width: 42px;"
                            Text="Go" OnClick="btngo_Click" />
                    </div>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblgradeval" Text="" Visible="false" runat="server" CssClass="font14"></asp:Label>
            <asp:Label ID="lblerrormsg" runat="server" Text="" Style="margin-left: 5px; width: auto;"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
                ForeColor="#FF3300"></asp:Label>
            <asp:Label ID="lblstuderrormsg" runat="server" Text="" Width="302px" Style="margin-left: 5px;"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
                ForeColor="#FF3300"></asp:Label>
            <br />
            <br />
            <center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded"
                    OnButtonCommand="Fpspread1_Command" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <asp:Label ID="lblErr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                    Font-Names="Book Antiqua"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btngrade" runat="server" Height="27px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Black" Text="Grade Sheet" Style="background-color: #e6e6e6;
                                box-shadow: 1px 11px 10px -11px; color: darkslategrey; border: 2px solid teal;"
                                OnClick="btngrade_Click" />
                            <asp:Button ID="btnrpt" runat="server" Height="27px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Black" Style="background-color: #e6e6e6; box-shadow: 1px 11px 10px -11px;
                                color: darkslategrey; border: 2px solid teal;" Text="Report Card" OnClick="btnrpt_Click" />
                            <asp:Button ID="btnmatric_page1" runat="server" Height="27px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Black" Style="background-color: #e6e6e6; box-shadow: 1px 11px 10px -11px;
                                color: darkslategrey; border: 2px solid teal;" Text="Report Card Page1" OnClick="btnmatric_page1_Click" />
                            <asp:Button ID="btnmatric_page2" runat="server" Height="27px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Black" Style="background-color: #e6e6e6; box-shadow: 1px 11px 10px -11px;
                                color: darkslategrey; border: 2px solid teal;" Text="Report Card Page2" OnClick="btnmatric_page2_Click" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </center>
            <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                Visible="true" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                CssClass="font14">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                Visible="false" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                CssClass="font14">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngrade" />
            <asp:PostBackTrigger ControlID="btnrpt" />
            <asp:PostBackTrigger ControlID="btnmatric_page1" />
            <asp:PostBackTrigger ControlID="btnmatric_page2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

