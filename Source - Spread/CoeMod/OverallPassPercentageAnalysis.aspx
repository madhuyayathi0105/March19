<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="OverallPassPercentageAnalysis.aspx.cs" Inherits="OverallPassPercentageAnalysis" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<html>
    <head>
        <title></title>
        <%--  <link href="Styles/OverAllpass.css" rel="stylesheet" type="text/css" />--%>
        <script type="text/javascript">
            function checkvalidate() {
                var checkvalidation1 = document.getElementById('<%=txt_degree.ClientID%>').value;
                var checkvalidation2 = document.getElementById('<%=txt_branch.ClientID%>').value;
                if (checkvalidation1 == "--Select--") {

                    alert("Please Select Degree"); gffdg
                    return false;

                }
                else if (checkvalidation2 == "--Select--") {

                    alert("Please Select Department");
                    return false;
                }

                else {

                    return true;
                }
            }
        </script>
    </head>
    <body>
        <div>
            
            
            
            
            
            
            
            
            
            
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
            <br />
                <center>
                   
                        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Large" Text="Over All Pass Percentage Analysis"
                            Style="font-family: Book Antiqua;"></asp:Label>
                       <%-- <asp:LinkButton ID="Iblback" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                             Style="font-family: Book Antiqua; position: absolute; margin-left: 250px;"
                            PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                        <asp:LinkButton ID="Iblhome" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                             Style="font-family: Book Antiqua; position: absolute; margin-left: 300px;"
                            PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
                        <asp:LinkButton ID="Ibllogout" Font-Size="Small" Font-Names="Book Antiqua" Font-Bold="true"
                            runat="server"  Style="position: absolute; margin-left: 350px;"
                            OnClick="lb2_Click">Logout</asp:LinkButton>--%>
                    
                </center>
                <br />
                
                    <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
                   
                      <table style="width:700px; height:70px; background-color:#0CA6CA;">
                            <tr>
                                <td>
                                    
                                    
                                    <asp:Label ID="Iblcollege" Text="College" Font-Bold="true" Style=""  Font-Names="Book Antiqua"
                                        Font-Size="Medium" runat="server">  </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" Style=""
                                        Font-Names="Book Antiqua" Font-Bold="true" Width="177px" Font-Size="Medium" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                        <asp:ListItem>--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="" 
                                        Font-Size="Medium"></asp:Label>
                                     
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                     
                                </td>
                                <td>
                                    <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                         Font-Size="Medium" Style="" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                <div style="position:relative">
                                      <asp:UpdatePanel ID="updpan_degree" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" Font-Bold="true" Font-Names="Book Antiqua" CssClass="Dropdown_Txt_Box"
                                        Style=" width: 100px;" runat="server"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Width="200px"
                                        BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px"  >
                                        <asp:CheckBox ID="chk_degree" runat="server" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="checkDegree_CheckedChanged"
                                            ForeColor="Black" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="Chklst_degree" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            ForeColor="Black" runat="server" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    </ContentTemplate>
                                    </asp:UpdatePanel></div>
                                </td>
                                <td>
                                    <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                         Font-Size="Medium" Style="" Text="Dept"></asp:Label>
                                </td>
                                <td>
                                <div style="position:relative">
                                      <asp:UpdatePanel ID="updpan_branch" runat="server">
                                        <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" Font-Bold="true" Font-Names="Book Antiqua" CssClass="Dropdown_Txt_Box"
                                        Style=" width: 100px;"
                                        runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" Width="300px"
                                        BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                        <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                                            ForeColor="Black" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            ForeColor="Black" runat="server" OnSelectedIndexChanged="chklst_branchselected"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                       </ContentTemplate>
                                    </asp:UpdatePanel></div>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                 Font-Size="Medium" Width="100px" Text="Sem From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsemfrom" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Style=" width: 42px;"
                                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsemfrom_SelectedIndexChanged1">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                 Font-Size="Medium"  width=" 82px" Text="Sem To"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsemto" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Style=" width: 42px;"
                                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsemto_SelectedIndexChanged1">
                                            </asp:DropDownList>
                                        </td>
                                        <td>                                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true"  Font-Size="Medium" OnClick="BtnGo_Click"
                                            OnClientClick=" return checkvalidate()" />
                                            </td>

                                    </tr>
                        </table>
                   
               
                
                <asp:Label ID="Label5" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" Style="margin-left: -726px;" ForeColor="Red" Width="800px"></asp:Label>
                <asp:Label ID="Label7" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" Style="margin-left: -473px;" ForeColor="Red" Width="800px"></asp:Label>
            </center>
            <center>
                
                
                
                <center>
                    <div>
                        <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"
                            OnRowCreated="grdover_RowCreated" ShowFooter="true" ShowHeader="true" OnDataBound="grdover_DataBound"
                            OnRowDataBound="grdover_OnRowDataBound" OnPreRender="grdover_PreRender">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex + 1%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="Control" />
                            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>
                    </div>
                </center>
            </center>
        </div>
        
        
        <center>
            
            <%-- <center>
         <asp:Label ID="lblHeading" runat="server" Font-Bold="true" Text="PASS PERCENTAGE ANALYSIS"
                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
         </center>--%>
            
            <center>
                <asp:Label ID="lblYear" runat="server" Font-Bold="true" ForeColor="Black" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            
            <div>
                <asp:Chart ID="Chart1" runat="server" Width="700px">
                    <Series>
                        <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea1" ChartType="Column">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
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
            <center>
                <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="Black" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            
            <div>
                <asp:Chart ID="Chart2" runat="server" Width="700px">
                    <Series>
                        <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea2" ChartType="Column">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea2">
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
            
            <center>
                <asp:Label ID="Label2" runat="server" Font-Bold="true" ForeColor="Black" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            
            <div>
                <asp:Chart ID="Chart3" runat="server" Width="700px">
                    <Series>
                        <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea3" ChartType="Column">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea3">
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
            
            <center>
                <asp:Label ID="Label3" runat="server" Font-Bold="true" ForeColor="Black" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            
            <div>
                <asp:Chart ID="Chart4" runat="server" Width="700px">
                    <Series>
                        <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea4" ChartType="Column">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea4">
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
        
        <center>
            <asp:Button ID="btnExcel" runat="server" Text="Export Excel" Font-Names="Book Antiqua"
                Font-Size="Medium" Font-Bold="true" Style="margin-right: 10px" OnClick="btnExcel_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" Style="margin-right: -500px" OnClick="btnPrint_Click" />
        </center>
    </body>
    </html>
</asp:Content>

