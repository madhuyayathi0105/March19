<%@ Page Title="Remark Entry" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="remarksentry.aspx.cs" Inherits="remarksentry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblexcelerror').innerHTML = "";
        }
        function get(txt1, maximumsubjectmark, mimisubjectmark) {

            var sst = document.getElementById(txt1).value;

            if (sst > maximumsubjectmark || sst < mimisubjectmark) {

                alert('Please Enter Correct Mark');
                document.getElementById(txt1).value = 0;
            }
        }
        function CheckAllsub(Checkbox) {

            var GridVwHeaderChckbox = document.getElementById("<%=gvatte.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
    <style>
        .style8
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .style10
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            width: 960px;
            height: 17px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 954px">
            <center>
                <asp:Label ID="Label1" runat="server" Text="Remark Entry" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Large" ForeColor="White"></asp:Label>
            </center>
        </asp:Panel>
        <div style="height: 53px; background-color: LightBlue; border-color: Black; border-style: solid;
            border-width: 1px; width: 954px;">
            <table style="margin-top: 10px;">
                <tr>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Height="20px" Width="59px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Bold="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="74px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            Width="190px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Visible="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 33px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Visible="true"
                            OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 21px; width: 44px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                            width: 47px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlactivity" runat="server" Visible="false" Style="margin-left: 10px;
                            width: 146px; background-color: #E1E3E4" AutoPostBack="true" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged"
                            CssClass="fontcomman">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Names="Book Antiqua"
                            ForeColor="Black" Font-Size="Medium" Style="height: 30px; width: 40px" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="22px"
            Style="width: 954px;">
        </asp:Panel>
    </center>
    <asp:Label ID="lblErrorMsg" runat="server" Text="" Style="font-family: Book Antiqua;
        color: Red; font-size: medium; font-weight: bold; height: 20px; margin-top: 27px;
        margin-bottom: 10px; position: relative; width: 263px;"></asp:Label>
    <center>
        <div id="show1" runat="server" style="margin-top: 10px; margin-bottom: 5px; position: relative;">
            <asp:Panel ID="pHeaderEntry" runat="server" CssClass="style8" Height="16px" Width="949px">
                <asp:Label ID="Labelpersonal" Text="Subject Details" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Image ID="Imagepersonal" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pnlEntry" runat="server" Height="215px" Width="835px">
                <center>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Visible="false" Height="307" Width="800" OnButtonCommand="FpSpread1_OnButtonCommand"
                                Style="margin-top: 5px; margin-bottom: 5px; position: relative;">
                                <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="false" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <asp:GridView ID="gvatte" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                Font-Bold="True" Font-Size="Medium" Style="font-family: Book Antiqua; border: 1px solid black;
                                margin-top: 5px; margin-bottom: 10px; position: relative;" BackColor="White" BorderColor="#CCCCCC"
                                BorderStyle="None" BorderWidth="1px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" ">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkallsubject" runat="server" onclick="CheckAllsub(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chksubject" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch Year">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbatch" runat="server" Text='<%# Eval("Batch_Year") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubj_name" runat="server" Text='<%# Eval("subject_name") %>'></asp:Label>
                                            <asp:Label ID="lblsub_ac" runat="server" Visible="false" Text='<%# Eval("acronym") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubcode" runat="server" Text='<%# Eval("subject_code") %>'></asp:Label>
                                            <asp:Label ID="lblsubno" runat="server" Visible="false" Text='<%# Eval("subject_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnok" runat="server" Text="OK" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="60px" OnClick="btnok_Click1" />
                </center>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlEntry"
                CollapseControlID="pHeaderEntry" ExpandControlID="pHeaderEntry" AutoExpand="true"
                AutoCollapse="false" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
        <div id="show2" runat="server" style="margin-top: 10px; margin-bottom: 5px; position: relative;">
            <div id="posalign" runat="server">
                <asp:Panel ID="pHeaderSettings" runat="server" CssClass="style10">
                    <asp:Label ID="Label4" Text="Student Details" runat="server" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlSettings" runat="server" BorderColor="Black" Height="308px" Width="907px">
                <center>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTest" runat="server" Visible="false" Text="Test" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTest" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                CssClass="pos" Style="font-family: Book Antiqua; font-size: 14; font-weight: bold;"
                                BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Kindly Enter report name"></asp:Label>
                    <asp:Button ID="btnfpspread1save" runat="server" Text="Save" CssClass="fontcomman"
                        Width="60px" OnClick="btnfpspread1save_Click1" />
                    <asp:Button ID="btnfpspread1delete" runat="server" Text="Delete" CssClass="fontcomman"
                        Width="90px" OnClick="btnfpspread1delete_Click1" />
                    <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                        Width="150px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnPrintrmk" runat="server" Text="Print" Width="100px" OnClick="btnPrintrmk_Click"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" />
                    <asp:GridView ID="gv" runat="server" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:GridView>
                    <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                        CssClass="stylefp">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <style>
                    .fontcomman
                    {
                        font-family: Book Antiqua;
                        font-size: medium;
                        font-weight: bold;
                    }
                    .pos
                    {
                        position: relative;
                        margin-top: 20px;
                    }
                </style>
                <center>
                    <table>
                        <tr>
                            <td style="width: auto;">
                                <asp:FileUpload runat="server" ID="fpmarkexcel" Visible="true" Font-Names="Book Antiqua"
                                    Font-Bold="True" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:Button ID="btn_import" Text="Import" runat="server" Visible="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_importex" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnlSettings"
                CollapseControlID="pHeaderSettings" ExpandControlID="pHeaderSettings" AutoCollapse="false"
                AutoExpand="true" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Image2"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
    </center>
    <%--Excess Confirmation --%>
    <center>
        <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_yes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                        <asp:Button ID="btn_no" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btn_sureno_Click" Text="no" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
