<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Subwise_Analy_rep.aspx.cs" Inherits="Subwise_Analy_rep" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <style>
        .fpoint
        {
            font-family: Book Antiqua;
            font-weight: bold;
            font-size: medium;
            color: Black;
        }
    </style>
    <script type="text/javascript">
        //        function chang(id) {
        //            var id_value = id.value;
        //            var val = document.getElementById('<%=first.ClientID %>');

        //            if (id_value == "Internal") {

        //                val.style.display = "block";
        //                //val1.style.display = "none";
        //            }
        //            if (id_value == "External") {
        //                // val1.style.display = "block";
        //                val.style.display = "none";
        //            }

        //        }
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }

        function validation() {
            var error = "";
            var branch = document.getElementById('<%=txtbranch.ClientID %>').value;
            var Test = document.getElementById('<%=txttest.ClientID %>').value;
            if (branch == "---Select---") {
                error += "Please Select  Branch \n";
            }
            if (Test == "---Select---") {
                error += "Please Select  TestName \n";
            }

            if (error.trim() == "") {
                return true;
            }
            else {
                alert(error);
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager><br /><center>
            <asp:Label ID="lbl" runat="server" Text="Subjectwise Analysis Report" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
 </center><br />
    <center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblclg" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="true" Width="230px" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlclg_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" Width="110px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" Width="110px"
                        Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlbatch_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" Width="110px"
                        Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                    <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                        ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                        Width="110px"></asp:TextBox>
                    <asp:Panel ID="pnlbranch" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                        Width="200px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="cbbranch" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbbranch_OnCheckedChanged" />
                        <asp:CheckBoxList ID="cblbranch" runat="server" Font-Size="Small" AutoPostBack="True"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Height="150px" OnSelectedIndexChanged="cblbranch_OnSelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <br />
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbranch"
                        PopupControlID="pnlbranch" Position="Bottom">
                    </asp:PopupControlExtender>
                     </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsem" runat="server" Text="Sem" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" Width="80px" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlsemselect">
                    </asp:DropDownList>
                    </td>
                    <td>
                    <asp:Label ID="lblsec" runat="server" Text="Sec" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                    <asp:DropDownList ID="ddlsec" runat="server" AutoPostBack="true" Width="80px" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlsecselect">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Repttype" runat="server" Text="Report Type" Style="font-weight: 600;"
                        font-name="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                       ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlrepttype" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" Width="110px" OnSelectedIndexChanged="ddlrepttype_OnSelectedIndexChanged">
                        <asp:ListItem>Internal</asp:ListItem>
                        <asp:ListItem>External</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Width="50px" Height="28px"
                        OnClick="btngo_OnClick" OnClientClick="return validation()" />
                </td>
            </tr>
 </table>
 <div style="width:870px; height:30px; background-color:#0CA6CA;">
        <table id="first" runat="server">
        <tr>
                    <td style="margin-left:1px">
                        <asp:Label ID="lblTestname" runat="server" Text="Test Name " font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txttest" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                            ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                           Width="190px"></asp:TextBox>
                        <asp:Panel ID="pnltest" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                            Width="200px">
                            <asp:CheckBox ID="cbtest" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbtest_OnCheckedChanged" />
                            <asp:CheckBoxList ID="cbltest" runat="server" Font-Size="Small" AutoPostBack="True"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Height="150px" OnSelectedIndexChanged="cbltest_OnSelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <br />
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txttest"
                            PopupControlID="pnltest" Position="Bottom">
                        </asp:PopupControlExtender>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table> 
            </div></center>
       
       
    <br />
    <asp:Label ID="lblmsg" runat="server" Visible="false" Text="No Records Found" ForeColor="Red"
        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
    <center>
        <div>
            <asp:GridView ID="internalgrid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                CellPadding="4" HeaderStyle-BackColor="CadetBlue" HeaderStyle-Font-Size="Medium"
                CssClass="Dropdown_Txt_Box" Width="800px" OnRowDataBound="internalgrid_OnRowDataBound"
                OnPreRender="internalgrid_OnPreRender" ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" Font-Names="Book Antiqua" />
                <HeaderStyle BackColor="#1C5E55" Font-Size="Medium" Font-Bold="True" ForeColor="Control"
                    Font-Names="Book Antiqua"></HeaderStyle>
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                CssClass="fpoint" Visible="false" BorderWidth="1px" VerticalScrollBarPolicy="Never"
                HorizontalScrollBarPolicy="Never">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
    </center>
    <br />
    <asp:Label ID="Label1" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="true"
        Font-Size="Medium" Font-Names="Book Antiqua" Style="margin-left: 81px; position: absolute;"></asp:Label>
    <br />
    <center>
        <div>
            <asp:Chart ID="Chart1" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                EnableViewState="true" Font-Size="Medium">
                <Series>
                </Series>
                <Legends>
                    <asp:Legend Title="Staff Performance" ShadowOffset="3" Font="Book Antiqua">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Docking="Bottom" Text="SUBJECT CODE">
                    </asp:Title>
                    <asp:Title Docking="Left" Text="PASS %">
                    </asp:Title>
                </Titles>
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
                        <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
    </center>
    <center>
        <div>
            <asp:GridView ID="Externalgrid" runat="server" Font-Bold="True" CellPadding="4" HeaderStyle-BackColor="CadetBlue"
                HeaderStyle-Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="800px" Visible="False"
                OnPreRender="Externalgrid_OnPreRender" OnRowDataBound="Externalgrid_OnRowDataBound"
                ForeColor="#333333" Font-Names="Book Antiqua">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" Font-Names="Book Antiqua" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" Font-Size="Medium" ForeColor="Control"
                    Font-Names="Book Antiqua" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </div>
    </center>
    <br />
    <br />
    <center>
        <div>
            <asp:Chart ID="Externalchart" runat="server" Width="700px" Height="500px" Palette="SeaGreen"
                EnableViewState="true" Visible="false" Enabled="false">
                <Series>
                    <asp:Series Name="Series2" XValueMember="subject code" YValueMembers="Pass%" IsValueShownAsLabel="true"
                        YValuesPerPoint="2" ChartArea="ChartArea2" ChartType="Column" Color="Teal">
                    </asp:Series>
                </Series>
                <Titles>
                    <asp:Title Alignment="TopCenter" Text="Subjectwise Performance Report" TextStyle="Shadow"
                        Font="Trebuchet MS, 20.25pt">
                    </asp:Title>
                    <asp:Title Docking="Bottom" Text="SUBJECT CODE" Font="15.15pt">
                    </asp:Title>
                    <asp:Title Docking="Left" Text="PASS %" Font="15.15pt">
                    </asp:Title>
                </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea2" BorderWidth="0">
                        <AxisY LineColor="Black">
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
    <table style="margin-left: 80px;">
        <tr>
            <td colspan="3">
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Visible="false" Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="Excel" runat="server" Text="Export Excel" Visible="false" Font-Size="Medium"
                    Font-Bold="true" Font-Names="Book Antiqua" OnClick="Excel_OnClick" />
            </td>
            <td>
                <asp:Button ID="Print" runat="server" Text="Print" Visible="false" Font-Size="Medium"
                    Font-Bold="true" Font-Names="Book Antiqua" OnClick="Print_OnClick" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

