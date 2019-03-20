<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Dummywithregno.aspx.cs" Inherits="Dummywithregno" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<style type="text/css">
         .stylefp
         {
             cursor:pointer;
             
         }
 .ModalPopupBG
{
    background-color: #666699;   
    filter: alpha(opacity=50);
    opacity: 0.7;
}

.HellowWorldPopup
{
    min-width:600px;
    min-height:400px;
    background:white;
   
}
.font
 {
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
        height: 27px;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br /><center>
            <asp:Label ID="Label1" runat="server" Text ="Assigning Dummy Number" 
            Font-Bold="True"  Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
         </center>        
        <br />
        <center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;">
    <tr>
       <td>
      <fieldset style="width:180px; height:23px" >
    <asp:RadioButton ID="Radioserial" GroupName="type" runat="server" Text="Serial" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"/>
        
    <asp:RadioButton ID="Radiorandom" GroupName="type" runat="server" Text="Random" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" /></fieldset>
              </td>
       <td>
       
       <asp:Label ID="lblMonthandYear" runat="server" CssClass="font" Text="Month & Year"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" 
               onselectedindexchanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True" 
               TabIndex="1"></asp:DropDownList>
       </td>
        <td>
       <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" 
                onselectedindexchanged="ddlYear_SelectedIndexChanged" AutoPostBack="True" 
                TabIndex="2"></asp:DropDownList>
       </td>
       <td>
       <asp:Label ID="Lbldate" runat="server" Text="Date" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
       </td>
        <td >
       <asp:DropDownList ID="ddldate" runat="server" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" 
               Width="100px" onselectedindexchanged="ddldate_SelectedIndexChanged" 
                TabIndex="3"></asp:DropDownList>
       </td>
       <td>
           <asp:Label ID="lblsubject" runat="server" Text="Subject" CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:DropDownList ID="ddlsubject" runat="server" CssClass="font" AutoPostBack="True" 
               Width="100px" onselectedindexchanged="ddlsubject_SelectedIndexChanged" 
               TabIndex="4">
           </asp:DropDownList>
       </td>
       </tr>
       </table> </center>
    <br />
      
       <table>
       <tr>
       <td>
           <asp:Label ID="lblregno" runat="server" Text="Register Number"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtreg" runat="server" CssClass="font" 
               ontextchanged="txtreg_TextChanged" AutoPostBack="true" TabIndex="5" ></asp:TextBox>
         
       </td>
        <td>
         <asp:Label ID="lbltotstud" runat="server" Text="No Of Students" CssClass="font"></asp:Label>
       </td>
              <td>
                  <asp:Label ID="lblviewstud" runat="server" CssClass="font" Text=""></asp:Label>
       </td>
       </tr>
       </table>
       
       <br />
       <table>
       <tr>
       <td>
           <asp:Label ID="lbldummyno" runat="server" Text="Dummy Number"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtdummy" runat="server" CssClass="font" AutoPostBack="true" 
               ontextchanged="txtdummy_TextChanged" TabIndex="6"></asp:TextBox>
           <asp:FilteredTextBoxExtender  runat="server" ID="txtdummyfilter" TargetControlID="txtdummy" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       <td>
       <asp:Label ID="lblremainstud" runat="server" CssClass="font" 
               Text="Dummy Number Generated" Visible="False"></asp:Label>
       </td>
       <td>
        <asp:Label ID="lblremainstudvies" runat="server" CssClass="font" Visible="False"></asp:Label>
       </td>
       
       </tr>

       </table>
       <br />
      <asp:Label ID="lblerrmag" runat="server" CssClass="font" ForeColor="Red" Text="" Visible="false" ></asp:Label>
       <br />
       <br />
       <table>
       <tr>
     
       
       </tr>
      </table>
      <center><table>
      <tr>
      <td>
          <FarPoint:FpSpread ID="sprdViewdummy" runat="server" BorderColor="Black" BorderStyle="Solid"
              BorderWidth="1px" Height="200" Width="400">
              <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                  ButtonShadowColor="ControlDark">
              </CommandBar>
              <Sheets>
                  <FarPoint:SheetView SheetName="Sheet1">
                  </FarPoint:SheetView>
              </Sheets>
          </FarPoint:FpSpread>
      </td>
      </tr>
      </table></center>
</asp:Content>

