<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="UnivresultAnalysis.aspx.cs" Inherits="UnivresultAnalysis" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/OverAllpass.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lblerrormsg.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function jsDecimals(e) {

            var evt = (e) ? e : window.event;
            var key = (evt.keyCode) ? evt.keyCode : evt.which;
            if (key != null) {
                key = parseInt(key, 10);
                if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
                    if (!jsIsUserFriendlyChar(key, "Decimals")) {
                        return false;
                    }
                }
                else {
                    if (evt.shiftKey) {
                        return false;
                    }
                }
            }
            return true;
        }
        function jsIsUserFriendlyChar(val, step) {
            // Backspace, Tab, Enter, Insert, and Delete  
            if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
                return true;
            }
            // Ctrl, Alt, CapsLock, Home, End, and Arrows  
            if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
                return true;
            }
            if (step == "Decimals") {
                if (val == 190 || val == 110) {  //Check dot key code should be allowed
                    return true;
                }
            }
            // The rest  
            return false;
            //                var TestVar = document.getElementById('Please Enter value Greater than Zero').value;
            //                alert(TestVar);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">University Result Analysis </span>
        <table style="width: auto; padding: 10px; height: auto; position: relative; margin: 0px;
            margin-top: 10px; margin-bottom: 15px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
            background-color: #0CA6CA;">
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                    AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sec">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Visible="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblTop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Top">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTop" runat="server" Font-Bold="true" AutoPostBack="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="2" onkeydown="return jsDecimals(event);" OnTextChanged="txtTop_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text=" ">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbconsolidatesubject" runat="server" AutoPostBack="True"
                                    RepeatDirection="Horizontal" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="rbconsolidatesubject_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="1">Consolidated </asp:ListItem>
                                    <asp:ListItem Value="2">Subject Wise </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbformat" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rbformat_SelectedIndexChanged" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                    <asp:ListItem Selected="True" Value="1">Format 1</asp:ListItem>
                                    <asp:ListItem Value="2">Format 2</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIncludeDiscontinue" runat="server" Text="Include Discontinue"
                                    Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbbeforeandafterrevaluation" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" RepeatDirection="Horizontal" Font-Size="Medium" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbbeforeandafterrevaluation_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="1">Before Revaluation </asp:ListItem>
                                    <asp:ListItem Value="2">After Revaluation </asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="rbmoderation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    RepeatDirection="Horizontal" Font-Size="Medium" AutoPostBack="true" Visible="false"
                                    OnSelectedIndexChanged="rbmoderation_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="1">Before Moderation </asp:ListItem>
                                    <asp:ListItem Value="2">After Moderation </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="lblCollegeHeaderName" runat="server" Text="College Header Name" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCollegeHeader" Text="" Font-Bold="true" Width="250px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblReportName" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportName" Text="" Font-Bold="true" Width="250px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkReportWithStream" runat="server" Text="Report Name with Stream"
                                    Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
                            </td>
                            <td>
                                <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="Buttongo_Click" Style="" Text="Go" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" Style="margin-left: -688px; margin-top: 10px;" ForeColor="Red"></asp:Label>
    <asp:Label ID="Label5" runat="server" Text="Error" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" Style="margin-left: -569px;" ForeColor="Red"></asp:Label>
    <asp:Label ID="Label2" runat="server" Text="Error" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" Style="margin-left: -495px;" ForeColor="Red"></asp:Label>
    <center>
        <div id="dvconsolidated" runat="server" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <div>
                <center>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" OnRowCreated="grd_RowCreated"
                        Width="800px" CellPadding="4" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="S.NO" HeaderText="S.NO" SortExpression="S.No" ReadOnly="true"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PARTICULARS" HeaderText="PARTICULARS" SortExpression="PARTICULARS"
                                ReadOnly="true" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" SortExpression="TOTAL" ReadOnly="true"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="G-DS" HeaderText="G-DS" SortExpression="G-DS" ReadOnly="true"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="G-HOSTEL" HeaderText="G-HOSTEL" SortExpression="G-HOSTEL"
                                ReadOnly="true" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="B-DS" HeaderText="B-DS" SortExpression="B-DS" ReadOnly="true"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="B-HOSTEL" HeaderText="B-HOSTEL" SortExpression="B-HOSTEL"
                                ReadOnly="true" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="Control" />
                        <PagerStyle CssClass="pgr" BackColor="#336666" HorizontalAlign="Center"></PagerStyle>
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                    </asp:GridView>
                </center>
            </div>
            <div style="margin: 0px; margin-bottom: 15px; margin-top: 8px; position: relative;">
                <center>
                    <asp:GridView ID="Totalgrd" runat="server" Width="800px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CellPadding="4" OnPreRender="Totalgrd_OnPreRender" OnRowCreated="Totalgrd_RowCreated">
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="Control" />
                        <PagerStyle CssClass="pgr" BackColor="#336666" HorizontalAlign="Center"></PagerStyle>
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                    </asp:GridView>
                </center>
            </div>
            <div style="margin: 0px; margin-bottom: 15px; margin-top: 15px; position: relative;">
                <center>
                    <asp:GridView ID="staffgvd" runat="server" AutoGenerateColumns="False" Width="800px"
                        Font-Bold="True" CellPadding="4" OnRowCreated="staffgvd_RowCreated" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="S.NO">
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex + 1%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SUBJECT CODE">
                                <ItemStyle HorizontalAlign="center" Width="50px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblsubcode" runat="server" Text='<%#Eval("SUBJECT CODE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STAFF NAME">
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblsat" runat="server" Text='<%#Eval("STAFF NAME")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SUBJECT NAME">
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblsubName" runat="server" Text='<%#Eval("SUBJECT NAME")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PASS %">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblpass" Width="40px" runat="server" Text='<%#Eval("PASS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="Control" />
                        <PagerStyle CssClass="pgr" BackColor="#336666" HorizontalAlign="Center"></PagerStyle>
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                    </asp:GridView>
                    <div>
                        <asp:Label ID="flow" runat="server" Text="STAFF PERFORMANCE" Font-Bold="true" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:Chart ID="Chart1" runat="server" Width="800px" Visible="true">
                            <Series>
                                <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea1" ChartType="Column">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </center>
            </div>
        </div>
        <div style="margin: 0px; margin-bottom: 10px; margin-top: 5px; position: relative;">
            <center>
                <asp:Button ID="btnExcel1" runat="server" Text="Export Excel" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" Style="margin-right: 10px" OnClick="btnExcel1_click" />
                <asp:Button ID="btnPrint1" runat="server" Text="Print" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" Style="margin-right: -500px" OnClick="btnPrint1_click" />
            </center>
        </div>
    </center>
   

     <center>
        <asp:Panel ID="pheaderfilter" runat="server"  CssClass="cpHeader" BackColor="#719DDB"
            Width="959px" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
        <asp:Panel ID="pbodyfilter" runat="server" CssClass="maintablestyle" Width="952px">
            <center>
                <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                    OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                    RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                    <asp:ListItem Text="Staff Name" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Designation" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Branch" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Sem & Sec" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="HIGHEST MARKS" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="LOWEST MARKS" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="CLASS AVERAGE" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Fail%" Selected="True"></asp:ListItem>
                    
                </asp:CheckBoxList>
            </center>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
            ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>

         <center>
        <div id="dvsubjectwise" runat="server" style="margin: 0px; margin-bottom: 20px; margin-top: 20px;
            position: relative;">
            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="1800px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
                <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" Visible="false">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AllowSort="false" GridLineColor="Black" BackColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div id="lastdiv" runat="server" style="margin: 0px; margin-bottom: 20px; margin-top: 15px;">
            <asp:Label ID="lblerrormsg" runat="server" Visible="false" Font-Names="Book Antiqua"
                Font-Size="Medium" Font-Bold="true" ForeColor="Red" Style="margin-left: 0px;"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name" Style="margin-left: 0px; margin-bottom: 20px;"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display1()" Height="20px"
                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:TextBox>
            <asp:Button ID="btnexcelsubject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Export To Excel" Width="127px" OnClick="btnexcelsubject_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </center>


        </center>
</asp:Content>
