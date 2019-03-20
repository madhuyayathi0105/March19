<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamvalidatorselectionMaster.aspx.cs" Inherits="ExamvalidatorselectionMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript">
    function display() {

        document.getElementById('MainContent_lblmessage1').innerHTML = "";

    }
    function make_blank() {
        document.form1.type.value = "";
    }
    </script>
    <style type="text/css">
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
    </style>
    <br /><center>
        <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Exam Validator Selection Master"></asp:Label></center>
 <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager><br />

    <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        runat="server" Width="1022px" Height="100px" BackColor="White" BorderColor="White"
        Style="background: White;">
        <Panes>
            <asp:AccordionPane ID="AccordionPane1" runat="server" TabIndex="1">
                <Header>View</Header>
                <Content>
                    <asp:Panel ID="panel1" runat="server">
                       <asp:Panel ID="Panel3" runat="server" Height="37px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Month And Year"></asp:Label>
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldate" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                      
                                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="font" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsession" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Branch"></asp:Label>
                                        <asp:DropDownList ID="ddlbranch" runat="server" CssClass="font" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Sem"></asp:Label>
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnView" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnView_Click" />
                                       
                                    </td>
                              
                                    <td>
                                      <fieldset style="border-style:solid; border-color:Black;  border-width:1px">
                                            <legend  font-names="Book Antiqua" font-size="Medium">Select</legend>
                                             <asp:RadioButtonList ID="RadioButtonList3" runat="server" CellSpacing="0" OnSelectedIndexChanged="RadioButtonList3_SelectedIndexChanged" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Horizontal" Style=" width: 210px; height: 12px;" Font-Bold="True">
                                            <asp:ListItem Value="1">Checked</asp:ListItem>
                                            <asp:ListItem Value="2">Un Checked</asp:ListItem>
                                            <asp:ListItem Value="3">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                       
                        <table>
                            <tr>
                                <asp:Label ID="lbleerrr" runat="server" Text="" Font-Bold="True" ForeColor="Red"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:Label>
                            </tr>
                        </table>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                        <FarPoint:FpSpread ID="fpspread" runat="server" 
                                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="550" Width="838px"
                                            HorizontalScrollBarPolicy="Never" Style="font-family: Book Antiqua; font-size: medium;
                                            font-weight: bold;" VerticalScrollBarPolicy="Never">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark" Visible="false">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </table>
                        </center>
                      
                    </asp:Panel>
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane2" runat="server" BackColor="White" TabIndex="2">
                <Header>
                    <asp:Label ID="AddPageModify" runat="server" Text="Add"></asp:Label></Header>
                <Content>
                    <center>
                        <asp:Panel ID="hmain_Panel" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Year And Month"></asp:Label>
                                        <asp:TextBox ID="txtyear" AutoPostBack="true" Enabled="false" Width="80px" runat="server"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:TextBox ID="txtmonth" AutoPostBack="true" Enabled="false" Width="80px" runat="server"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:TextBox ID="txtdegre" AutoPostBack="true" Enabled="false" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Branch"></asp:Label>
                                        <asp:TextBox ID="txtbaranch" AutoPostBack="true" Enabled="false" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Sem"></asp:Label>
                                        <asp:TextBox ID="txtsem" AutoPostBack="true" Enabled="false" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject Code And Name" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True">
                                            <asp:TextBox ID="txtsubjects" AutoPostBack="true" Width="400px" Enabled="false" runat="server"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                            <asp:Label ID="lbltolno" runat="server" Text="Total No Of Student" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="True"></asp:Label>
                                            <asp:TextBox ID="txttplstudent" AutoPostBack="true" Enabled="false" Width="80px"
                                                runat="server" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblfrom" runat="server" Text=" Date" Style="font-family: 'Baskerville Old Face';
                                                font-weight: 700;" Width="36px" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="16px"></asp:Label>
                                            <asp:TextBox ID="txtDate" CssClass="txt" runat="server" Height="19px" Width="90px" OnTextChanged="txtDate_OnTextChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtDate" runat="server"
                                                Format="d-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </asp:Label><asp:Label ID="lblsuuuuno" runat="server" Visible="false" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltolpaperper" runat="server" Text="No Of Validation Papers ( Per Person )"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:Label>
                                        <asp:TextBox ID="txttolperval" runat="server" OnTextChanged="txttolperval_OnTextChanged" MaxLength="3"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="lblnoval" runat="server" Text=" No Of Validator" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                                        <asp:TextBox ID="txtnoofval" runat="server" OnTextChanged="txtnoofval_OnTextChanged" MaxLength="2"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="lblEvaluation" runat="server" Text="Evaluation" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                                        <asp:DropDownList ID="drpevl" runat="server" AutoPostBack="True" Font-Bold="True"
                                            OnSelectedIndexChanged="drpevl_SelectedIndexChanged" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="left: -16px;
                                position: absolute; width: 1040px; top: 365px; height: 10px">
                            </asp:Panel>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblerr1" runat="server" Text="" Font-Bold="True" Style="margin-left: 0px;
                                            top: 377px; left: 11px; width: 1058px; position: absolute;" ForeColor="Red" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="tolnoremaion" Visible="false" Style="position: absolute; left: 630px;
                                            top: 385px;" runat="server" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                            ForeColor="#5078B3"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                        <br />
                                        <FarPoint:FpSpread ID="AttSpread" Visible="false" runat="server" OnUpdateCommand="AttSpread_OnUpdateCommand"
                                            ActiveSheetViewIndex="0" currentPageIndex="0">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                            </CommandBar>
                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False"></Pager>
                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False"></HierBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="#DEDFDE"
                                                    SelectionBackColor="#CE5D5A" SelectionForeColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False">
                                            </TitleInfo>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnsavel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Visible="false" OnClick="btnsavel1_click" Text="Save" />
                                        &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:Button ID="btnreet1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Visible="false" OnClick="btnreet1_Click" Text="Reset" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </Content>
            </asp:AccordionPane>
        </Panes>
    </asp:Accordion>
</asp:Content>

