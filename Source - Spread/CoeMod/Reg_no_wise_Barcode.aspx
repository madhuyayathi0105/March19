<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Reg_no_wise_Barcode.aspx.cs" Inherits="Reg_no_wise_Barcode" %>

<%@ Register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .txt
        {
            top: 123px;
            left: 118px;
            position: absolute;
            height: 18px;
            width: 80px;
            right: 797px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
   <br /><center>
                      <asp:Label ID="lblhead" runat="server" Text="Barcode Generation" Font-Bold="True" 
                         Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green" /></center>
                      
                   
 <br /><center>
   <table style="width:700px; height:70px; background-color:#0CA6CA;"
>
                 <tr>
                     <td >
                    
                         <asp:Label runat="server" ID="lblbatch" Text="Batch"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                         </td>
                         <td>
                  <asp:DropDownList ID="ddlbatch" runat="server"  Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" 
         onselectedindexchanged="ddlbatch_SelectedIndexChanged" Height="25px" 
         Width="61px" AutoPostBack="True">
                         </asp:DropDownList>
                     </td>
                     <td >
                    <asp:Label runat="server" ID="lbldegree" Text="Degree"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                         </td>
                         <td>
                         <asp:DropDownList runat="server" ID="ddldegree"  Height="25px" Width="90px"   
         AutoPostBack="True" 
         onselectedindexchanged="ddldegree_SelectedIndexChanged"  Font-Bold="True" 
                             Font-Names="Book Antiqua" Font-Size="Medium" >
                         </asp:DropDownList>
                     </td>
                     <td >
             <asp:Label runat="server" ID="lblbranch" Text="Branch"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
             </td>
                  <td>   
                         <asp:DropDownList runat="server" ID="ddlbranch"  Font-Bold="True" Height="25px" Width="150px" 
         Font-Names="Book Antiqua" Font-Size="Medium"
         onselectedindexchanged="ddlbranch_SelectedIndexChanged" 
         AutoPostBack="True">
                         </asp:DropDownList>
                     </td>
                     <td>
              <asp:Label runat="server" ID="lblduration" Text="Sem"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
               </td>
                    <td>
                         <asp:DropDownList runat="server" ID="ddlduration"  Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"
         AutoPostBack="True" Height="25px" Width="40px" 
         onselectedindexchanged="ddlduration_SelectedIndexChanged"  >
                         </asp:DropDownList>
                     </td>
                     <td>
                 <asp:Button ID="btnGo" runat="server" Text="Go" 
                                style="font-weight: 700" onclick="btnGo_Click" 
         Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" 
         Width="36px"/>
                    </td>
                    <td></td>
                 </tr>

                 </table></center>
                 <br />
    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="#FF3300" 
            Text="No Record(s) Found" CssClass="style2"></asp:Label>
  <br />
    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" 
                      BorderStyle="Solid" BorderWidth="1px" Height="200">
                      <CommandBar BackColor="Control" ButtonFaceColor="Control" 
                          ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" ButtonType="PushButton">
                      </CommandBar>
                      <Sheets>
                          <FarPoint:SheetView SheetName="Sheet1" AutoPostBack=True >
                          </FarPoint:SheetView>
                      </Sheets>
                  </FarPoint:FpSpread>
</asp:Content>

