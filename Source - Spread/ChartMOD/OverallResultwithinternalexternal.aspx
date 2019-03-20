<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master" AutoEventWireup="true" CodeFile="OverallResultwithinternalexternal.aspx.cs" Inherits="OverallResultwithinternalexternal" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            width: 763px;
        }
        .style2
        {
            width: 154px;
        }
        .style3
        {
            width: 116px;
        }
        .style4
        {
            width: 328px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblvalidation1').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager><br /><center>
        <asp:Label ID="lblhead" runat="server" Text="Overall Result With Internal and External Marks Comparison"
                CssClass="fontstyleheader" ForeColor="Green"></asp:Label></center>
 <br /><center>
        <table style="width:900px; height:40px;" class="maintablestyle ">
            <tr>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Exam Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True"
                        Width="61px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Exam Month" Width="95px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True"
                        Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                        Width="61px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="90px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlbrach_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" Style="font-weight: 700" OnClick="btnGo_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" Width="36px" />
                </td>
            </tr>
        </table>
       </center>
    <br />
    <asp:Label ID="errorlabl" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <br />
    <br />
    <center>
        <div>
            <div>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <div>
                <asp:Chart ID="Chart1" runat="server" Width="1000px" Visible="false" Font-Names="Book Antiqua"
                    EnableViewState="true" Font-Size="Medium">
                    <Series>
                    </Series>
                    <Legends>
                        <asp:Legend Title="Staff Performance" ShadowOffset="2" Font="Book Antiqua">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Docking="Bottom" Text="SUBJECT CODE">
                        </asp:Title>
                        <asp:Title Docking="Left" Text="AVERAGE OF MARKS">
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
        </div>
    </center>
    <br />
    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
    <br />
    <div id="rptprint" runat="server" visible="false">
        <center>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <%--        <asp:Button ID="btndummynoprint" runat="server" Text="Dummy Number Print" OnClick="btndummynoprint_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />--%>
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </center>
    </div>
</asp:Content>

