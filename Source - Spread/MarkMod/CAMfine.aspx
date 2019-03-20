<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAMfine.aspx.cs" Inherits="CAMfine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style5
        {
            width: 45px;
        }
        .style6
        {
            width: 39px;
        }
        .style14
        {
            width: 38px;
        }
        .style15
        {
            width: 104px;
        }
        .style18
        {
            width: 11px;
        }
        .style19
        {
            height: 11px;
        }
        .style20
        {
            width: 100px;
            height: 11px;
        }
        .style21
        {
            height: 33px;
            position: absolute;
            left: 698px;
            top: 504px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <body oncontextmenu="return false">
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_lblnorec').innerHTML = "";
            }
        </script>
        <center>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Panel ID="Test" runat="server" BorderColor="White" BorderStyle="Dotted" ClientIDMode="Static"
                    Width="1000px" Style="margin-top: 59px; height: auto;">
                    <table>
                        <tr>
                            <td align="left">
                                <center>
                                    <center>
                                        <asp:Label ID="Label1" runat="server" Text="CAMR2-Monthly And Model Examination Fine Reports"
                                            CssClass="fontstyleheader" ForeColor="green"></asp:Label>
                                    </center>
                                </center>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnls1" runat="server" Style="background-color: #0CA6CA; width: 954px;
                        border: 1px solid black; border-bottom-width: 0; margin-left: 0px" Height="27px">
                        <table>
                            <tr>
                                <td class="style5">
                                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 40px">
                                    <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Height="23px"
                                        OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Degree"></asp:Label>
                                </td>
                                <td class="style18">
                                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="23px"
                                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="103px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style15">
                                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="23px"
                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="250px" Style="margin-left: 0px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 30px">
                                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem" runat="server" AutoPostBack="True" Height="23px" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                                        Width="43px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 30px">
                                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                        Height="23px" Width="70px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td class="style19">
                                    <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Test"></asp:Label>
                                </td>
                                <td class="style20">
                                    <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="23px" Width="140px" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl2" runat="server" Style="background-color: #0CA6CA; margin-top: 0px;
                        width: 954px; border: 1px solid black; border-bottom: 0px; border-top: 0px;"
                        Height="30px">
                        <table>
                            <tr>
                                <td class="style19">
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Subjects"></asp:Label>
                                </td>
                                <td class="style19">
                                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" EnableTheming="False"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"
                                        Height="17px" OnInit="TextBox1_Init" OnTextChanged="TextBox1_TextChanged1" Width="254px"></asp:TextBox>
                                </td>
                                <td class="style19">
                                    <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px" Style="width: 195px;
                                        height: 21px; position: absolute; top: 4px; left: 344px;">
                                        <asp:RadioButton ID="Unitbtn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" GroupName="Test" Text="Unit Test" />
                                        <asp:RadioButton ID="terminalbtn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" GroupName="Test" Text="Terminal Test" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderWidth="1px" Style="width: 195px;
                                        height: 21px; position: absolute; top: 4px; left: 546px;">
                                        <asp:RadioButton ID="SubjectRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" GroupName="Test2" Text="Subject Name" />
                                        <asp:RadioButton ID="acronymradio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" GroupName="Test2" Text="Acronym" />
                                    </asp:Panel>
                                </td>
                                <td style="width: 45px">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="left: 752px; margin-top: -12px; position: absolute;"
                                        Text="Range"></asp:Label>
                                </td>
                                <td style="width: 20px">
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Style="left: 800px;
                                        margin-top: -12px; position: absolute;" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                </td>
                                <td style="width: 65px">
                                    <asp:DropDownList ID="Ddlrange" runat="server" AutoPostBack="True" Style="left: 830px;
                                        margin-top: -12px; position: absolute;" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="22px" Width="65px">
                                        <asp:ListItem>Absentees</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 45px">
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" Text="Go" Style="left: 906px; margin-top: -14px;
                                        position: absolute;" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnls4" runat="server" Style="background-color: #0CA6CA; margin-top: 0px;
                        width: 954px; border: 1px solid black; border-top: 0px;" Height="34px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="Finesettings" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Underline="True" OnClick="Finesettings_Click" Text="Fine Settings"
                                        Width="108px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                        Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panels" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="300px"
                        Direction="LeftToRight" ScrollBars="Auto" BackColor="White">
                        <asp:CheckBoxList ID="ddlSubject" runat="server" AutoPostBack="True" Font-Bold="True"
                            OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged1">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="panels"
                        DynamicServicePath="" Enabled="true" TargetControlID="TextBox1">
                    </asp:DropDownExtender>
                    <asp:Label ID="Label10" runat="server" ForeColor="#FF3300" Text="Select Subject"
                        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                    <br />
                    <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
                        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:Label>
                    <asp:Panel ID="pnlEntry" runat="server" Style="margin-left: -63px;">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" ActiveSheetViewIndex="0" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px" ClientIDMode="AutoID" currentPageIndex="0"
                                                OnPreRender="FpSpread1_SelectedIndexChanged" HorizontalScrollBarPolicy="Never"
                                                VerticalScrollBarPolicy="Never" Height="130px" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                                OnActiveRowChanged="FpSpread1_ActiveRowChanged" OnActiveSheetChanged="FpSpread1_ActiveSheetChanged"
                                                OnCellClick="FpSpread1_CellClick" OnEditCommand="FpSpread1_EditCommand" OnSaveOrLoadSheetState="FpSpread1_SaveOrLoadSheetState"
                                                Visible="False" Width="370px" ShowHeaderSelection="false">
                                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                    ButtonShadowColor="ControlDark">
                                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                </CommandBar>
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" AutoPostBack="false">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                                    VerticalAlign="NotSet">
                                                </TitleInfo>
                                            </FarPoint:FpSpread>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="Button8" runat="server" OnClick="Button5_Click" TabIndex="1" Text="Save"
                                                Visible="False" Font-Bold="true" />
                                            <asp:Button ID="Button6" runat="server" OnClick="Button6_Click" Text="Exit" Visible="False"
                                                Font-Bold="true" />
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Visible="False"
                                            Font-Names="Book Antiqua"></asp:Label>
                                        <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="     Records Per Page"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        &nbsp;<asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged" Font-Bold="True"
                                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px" Width="58px">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="TextBoxother" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" OnTextChanged="TextBoxother_TextChanged"
                                            Visible="false" Width="34px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxother"
                                            FilterType="Numbers" />
                                        <asp:Label ID="lblpage" runat="server" Visible="False" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Page Search:" Width="122px"></asp:Label>
                                        &nbsp;<asp:TextBox ID="TextBoxpage" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="17px" OnTextChanged="TextBoxpage_TextChanged"
                                            Visible="False" Width="34px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBoxpage"
                                            FilterType="Numbers" />
                                        <asp:Label ID="LabelE" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center style="margin-left: -22px">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <FarPoint:FpSpread ID="FpEntry" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                                                        currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                                        EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                                                        Visible="False" ShowHeaderSelection="false">
                                                        <CommandBar BackColor="Control" ButtonType="PushButton">
                                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                        </CommandBar>
                                                        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" />
                                                        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" />
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                                                GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                                                SelectionForeColor="White">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                                            VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                            Font-Strikeout="False" Font-Underline="False">
                                                        </TitleInfo>
                                                    </FarPoint:FpSpread>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                            Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                        <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnmasterprint_Click" />
                                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
