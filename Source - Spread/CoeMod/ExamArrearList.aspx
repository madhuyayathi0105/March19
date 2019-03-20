<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamArrearList.aspx.cs" Inherits="ExamArrearList" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<style type="text/css">
    .font
 {
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
        }
 
</style><br />
<center>
<asp:Label ID="Label7" runat="server" Text ="Arrear Status" 
            Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
  <br />
  <center>
       <table style="width:700px; height:70px; background-color:#0CA6CA;">
       <tr>
         <td>
        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="font"></asp:Label>
       </td>
       <td>
        <asp:DropDownList ID="ddlbatch" runat="server"  CssClass="font" Width="70px" 
               AutoPostBack="True" onselectedindexchanged="ddlbatch_SelectedIndexChanged">
           </asp:DropDownList>
       </td>
       <td>
        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="font"></asp:Label>
       </td>
       <td>
         <asp:DropDownList ID="ddldegree" runat="server"  CssClass="font" Width="100px" 
               AutoPostBack="True" onselectedindexchanged="ddldegree_SelectedIndexChanged">
           </asp:DropDownList>
       
       </td>
      
       <td>
        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="font"></asp:Label>
       </td>
        <td>
            <asp:DropDownList ID="ddlbranch" runat="server" CssClass="font" Width="170px" 
                AutoPostBack="True">
              
            </asp:DropDownList>
       </td>
      <td>
      <asp:Label ID="lblarrearcount" runat="server" Text="No Of Arrear" CssClass="font" Width="100px"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtnoofarr" runat="server" CssClass="font" 
               ontextchanged="txtnoofarr_TextChanged" Width="40px" AutoPostBack="True"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtnoofarr"></asp:FilteredTextBoxExtender>
       </td>
       <td>
         <asp:Button ID="btngenerate" runat="server" Text="Go" 
                                 onclick="btngenerate_Click" 
         Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" 
         Width="36px" style="margin-left: 1px"/>
       </td>
       </tr>
       </table>
      </center>
       <br />
       
    <asp:Label ID="lblnorec" runat="server" Text="No Recors Found" CssClass="font" Visible="false" ForeColor="Red"></asp:Label>
       <br />
       <br />
      

    <FarPoint:FpSpread ID="sprdviewrcrd" runat="server" BorderColor="Black" 
    BorderStyle="Solid" BorderWidth="1px" Height="200" oncellclick="sprdviewrcrd_CellClick" 
    OnPreRender="sprdviewrcrd_SelectedIndexChanged">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" 
        ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" ButtonType="PushButton" >
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>

     <center>
                
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" 
                      BorderStyle="Solid" BorderWidth="1px" Height="200" >
                      <CommandBar BackColor="Control" ButtonFaceColor="Control" 
                          ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" ButtonType="PushButton">
                      </CommandBar>
                      <Sheets>
                          <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                          </FarPoint:SheetView>
                      </Sheets>
                  </FarPoint:FpSpread>
                  </center>
</asp:Content>

