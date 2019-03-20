<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Beforeandafterrevaluation.aspx.cs" Inherits="Beforeandafterrevaluation" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager><br />
    <center>
     <asp:Label ID="Label2" runat="server" Text="Comparision Of Results (Before And After Revaluation)" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
         <br />
         <center>
            <table style="width:700px; height:70px; background-color:#0CA6CA;"
>
    <tr>
    <td>
<asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
</td>
        <td >

<asp:DropDownList ID="ddl_college" runat="server" Font-Names="Book Antiqua" Font-Size="Medium" Width="218px"></asp:DropDownList>
                
</td>
               <td>
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                CausesValidation="True">
                            </asp:DropDownList>
                                                    
                                     
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CausesValidation="True">
                            </asp:DropDownList>
                            
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"> </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CausesValidation="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" 
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            <br />
                        </td>
                    </tr>
   
    <tr>
     <td>
         <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
         <td>
             <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                            </asp:DropDownList>
                            <br />
                        </td>
                  <td>
                            <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                            </asp:DropDownList>
                            <br />
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
    </tr>
    </table>
     </center>
     <br />
      <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"  Text=" " Visible="False"></asp:Label>
                            <br />
                            <center>
                              <FarPoint:FpSpread ID="FpExternal" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="900px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
                <CommandBar ShowPDFButton="false" ButtonType="PushButton" Visible="true">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AllowSort="false" GridLineColor="Black" BackColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            
           <br />
            <asp:Label ID="lblerrormsg" runat="server" Visible="false" Font-Bold="true" ForeColor="Red" style="position: absolute;left:50px;"></asp:Label>
            <br />
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                  
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>

                            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />

                            <asp:Button ID="btnprintmaster" runat="server" Text="Print"  
                                onclick="btnprintmaster_Click"   Font-Names="Book Antiqua" Font-Size="Medium"  Font-Bold="true" />
                            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                            </center>
</asp:Content>

