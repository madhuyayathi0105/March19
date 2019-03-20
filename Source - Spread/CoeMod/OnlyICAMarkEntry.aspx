<%@ Page Title="Only ICA Mark Entry" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="OnlyICAMarkEntry.aspx.cs" Inherits="OnlyICAMarkEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblexcelerror').innerHTML = "";
        }
        //        function get(txt1, maximumsubjectmark, mimisubjectmark) {

        //            var sst = document.getElementById(txt1).value;

        //            if (sst > maximumsubjectmark || sst < mimisubjectmark) {

        //                alert('Please Enter Correct Mark');
        //                document.getElementById(txt1).value = '';
        //            }
        //        }
        function CheckAllsub(Checkbox) {

            var GridVwHeaderChckbox = document.getElementById("<%=gvatte.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js">       
    </script>
    <script type="text/javascript">
        function InitEvents() {
            $("table[id*=gvmarkentry] input[id*=txtm1]").blur(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm1]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm1]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark1]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    $(this).focus();
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm2]").blur(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm2]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm2]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark2]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm3]").blur(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm3]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm3]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark3]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm4]").blur(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm4]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm4]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark4]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm5]").blur(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm5]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm5]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark5]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    $(this).focus();
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm6]").focusout(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm6]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm6]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark6]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
            $("table[id*=gvmarkentry] input[id*=txtm7]").focusout(function (e) {
                var internalMark = $(this).closest("tr").find("input[id*=txtm7]").val();
                var minInternal = $(this).closest("tr").find("input[id*=txtm7]").val();
                var maxnternal = $(this).closest("tr").find("input[id*=txtMaxMark7]").val();
                if (internalMark == "") {
                    return true;
                }
                else if (internalMark.includes('a') || internalMark.includes('A')) {
                    return true;
                }
                else if (parseFloat(internalMark) > parseFloat(maxnternal)) {
                    $(this).val('');
                    alert("Internal Mark Must be Less Than or Equal to " + maxnternal);
                    $(this).focus();
                    return false;
                }
                return true;
            });
        }
        $(document).ready(InitEvents);
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
            top: 190px;
            left: 0px;
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitEvents);
    </script>
    <div style="position: relative; margin: 0px; margin-bottom: 25px; width: 100%; height: auto;">
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 100%;
            height: 25px;">
            <center>
                <asp:Label ID="Label1" runat="server" Text="Only ICA Mark Entry" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="White"></asp:Label>
            </center>
        </asp:Panel>
        <center>
            <div style="height: auto; background-color: LightBlue; border-color: Black; border-style: solid;
                border-width: 1px; width: 100%; position: relative;">
                <table style="margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblExamMonthYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Exam Year And Month"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged" Font-Size="Medium"
                                Width="60px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamMonth" runat="server" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
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
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                Width="190px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Visible="true"
                                Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 33px;"></asp:Label>
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
                            <div style="position: relative;">
                                <asp:DropDownList ID="ddlSec" Visible="false" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 47px;">
                                </asp:DropDownList>
                                <asp:UpdatePanel ID="UpnlSec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSec" Width="60px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                            <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtSec" runat="server" TargetControlID="txtSec"
                                            PopupControlID="pnlSec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
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
        </center>
        <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 100%;
            height: 22px; position: relative;">
        </asp:Panel>
        <asp:Label ID="lblErrorMsg" runat="server" Text="" Style="font-family: Book Antiqua;
            color: Red; font-size: medium; font-weight: bold; height: auto; margin-top: 27px;
            position: relative; margin-bottom: 10px; width: 263px;"></asp:Label>
        <div id="show1" runat="server" style="margin-top: 10px; margin-bottom: 12px; position: relative;">
            <asp:Panel ID="pHeaderEntry" runat="server" CssClass="style8" Height="16px" Width="949px">
                <%--&nbsp;Personal Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                <asp:Label ID="Labelpersonal" Text="Subject Details" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagepersonal" runat="server" CssClass="cpimage" ImageUrl="~/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pnlEntry" runat="server" Height="215px" Width="835px" Style="margin-top: 5px;
                position: relative;">
                <center>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Visible="false" Height="307" Width="800" OnButtonCommand="FpSpread1_OnButtonCommand">
                                <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="false" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <asp:GridView ID="gvatte" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                Font-Bold="True" Font-Size="Medium" Style="font-family: Book Antiqua; border: 1px solid black;"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
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
                                            <asp:Label ID="lblmaxmark" runat="server" Visible="false" Text='<%# Eval("maxtotal") %>'></asp:Label>
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
                    <br />
                    <br />
                    <asp:Button ID="btnok" runat="server" Text="OK" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="60px" Style="height: auto; margin-bottom: 10px;" OnClick="btnok_Click1" />
                </center>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlEntry"
                CollapseControlID="pHeaderEntry" ExpandControlID="pHeaderEntry" AutoExpand="true"
                AutoCollapse="false" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
                CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
        <br />
        <div id="show2" runat="server" style="margin-top: 15px; position: relative;">
            <div id="posalign" runat="server">
                <asp:Panel ID="pHeaderSettings" runat="server" CssClass="style10">
                    <asp:Label ID="Label4" Text="Student Details" runat="server" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua" />
                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="~/right.jpeg" />
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlSettings" runat="server" BorderColor="Black" Height="308px" Width="907px">
                <center>
                    <br />
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
                    <asp:GridView ID="gvmarkentry" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvmarkentry_RowDataBound"
                        BackColor="White" Font-Bold="True" Font-Size="Medium" Style="font-family: Book Antiqua;
                        border: 1px solid black;" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roll No">
                                <ItemTemplate>
                                    <asp:Label ID="lblroll" runat="server" Text='<%# Eval("roll_no") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblstud_name" runat="server" Text='<%# Eval("stud_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblsc1" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark1" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark1" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm1" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtm1"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc2" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark2" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark2" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm2" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtm2"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc3" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark3" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark3" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm3" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtm3"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc4" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark4" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark4" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm4" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtm4"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc5" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark5" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark5" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm5" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtm5"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc6" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark6" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark6" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm6" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtm6"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <asp:Label ID="lblsc7" runat="server" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMaxMark7" CssClass="maxMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtMinMark7" CssClass="minMark" runat="server" Style="display: none;"></asp:TextBox>
                                    <asp:TextBox ID="txtm7" CssClass="internalMark" runat="server" Width="50px" Style="text-align: center;
                                        border: 0px; background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtm7"
                                        FilterType="Numbers,Custom" ValidChars="Aa">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
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
                    <br />
                    <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Kindly Enter report name"></asp:Label>
                    <br />
                    <asp:Button ID="btnfpspread1save" runat="server" Text="Save" CssClass="fontcomman"
                        Width="60px" OnClick="btnfpspread1save_Click1" OnClientClick="javascript:return ValidateMarks();" />
                    <asp:Button ID="btnfpspread1delete" runat="server" Text="Delete" CssClass="fontcomman"
                        Width="90px" OnClick="btnfpspread1delete_Click1" Visible="false" />
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
                <br />
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
            <br />
            <br />
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnlSettings"
                CollapseControlID="pHeaderSettings" ExpandControlID="pHeaderSettings" AutoCollapse="false"
                AutoExpand="true" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Image2"
                CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
    </div>
</asp:Content>
