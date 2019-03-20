<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master" AutoEventWireup="true" CodeFile="test_subject_chart.aspx.cs" Inherits="test_subject_chart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <br /><center>
        <asp:Label ID="lblhead" runat="server" Text="Test Wise And Subject Wise Chart" CssClass="fontstyleheader" ForeColor="Green"></asp:Label></center>
   <br />
   <center>
   <table style="width:900px; height:70px; " class="maintablestyle" >
         <tr>
                <td>
                  <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" Height="27px" Width="73px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="25px"
                        Width="82px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td></tr>
         <tr>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="24px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style32"
                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="200px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True" Style="height: 24px;
                        width: 47px;" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" S Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" runat="server" Height="22px" Width="43px" AutoPostBack="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged">
                    </asp:DropDownList>
                </td></tr>
        <tr>
                <td>
                    <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px">
                        <asp:RadioButtonList ID="radiobutton1" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="radiobutton1_selectedindexchanged"
                            AutoPostBack="true">
                            <asp:ListItem Selected>CAM Wise</asp:ListItem>
                            <asp:ListItem>University Wise</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="Panel4" runat="server" BorderColor="Black" BorderWidth="1px">
                        <asp:RadioButton runat="server" ID="rdTestWise" Text="TestWise" GroupName="Test/Subject"
                            Font-Bold="True" AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnCheckedChanged="rdTestWise_CheckedChanged" />
                        <asp:RadioButton ID="rdSubWise" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="Test/Subject" Text="SubjectWise"
                            OnCheckedChanged="rdSubWise_CheckedChanged" />
                    </asp:Panel>
                </td>
                <td>
                    <asp:Label ID="lblTest" runat="server" Text="Test" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_test" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="125px">--Select--</asp:TextBox>
                            <asp:Panel ID="panel_Department" runat="server" Height="100px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="125px" ScrollBars="Auto">
                                <asp:CheckBox ID="chktesr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktesr_checkedchanged" />
                                <asp:CheckBoxList ID="ddlTest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlTest_selectedchanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_test"
                                PopupControlID="panel_Department" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </td>
                <td>    
               <asp:Button ID="btngo" runat="server" Text="Go" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="True" OnClick="btngo_Click" />
                </td>
         </tr>
        </table></center>
   <br />
     <asp:Panel ID="Panel3" runat="server">
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblErrTest" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="style14" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="panel5" runat="server" Width="1000px">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="panelchart" runat="server" Width="1000px">
                    </asp:Panel>
                    <%--<asp:Chart ID="Chart1" runat="server" Width="1100">
                        <Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Name="legend1">
                            </asp:Legend>
                        </Legends>
                    </asp:Chart>--%>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

