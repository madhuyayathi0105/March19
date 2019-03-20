<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Exam_Markconvertion.aspx.cs" Inherits="Exam_Markconvertion" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<style type="text/css">
    .font
{
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
       
    }
</style>
 <asp:ScriptManager ID="scrptmngr" runat="server">
 </asp:ScriptManager><br />
 <center>
  <asp:Label ID="Label7" runat="server" Text ="Mark Conversion" 
            Font-Bold="True"  Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green" 
                     meta:resourcekey="Label7Resource1"></asp:Label>
            </center>   
            <br />
            <center>
       <table style="width:700px; height:70px; background-color:#0CA6CA;">
       <tr>
        <td>
         <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" 
              Font-Names="Book Antiqua" Font-Size="Medium" 
                meta:resourcekey="lblbatchResource1"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="True" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
              onselectedindexchanged="ddlbatch_SelectedIndexChanged" Width="100px" 
                meta:resourcekey="ddlbatchResource1"></asp:DropDownList>
        </td>
      <td>
      <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" 
              Font-Names="Book Antiqua" Font-Size="Medium" 
              meta:resourcekey="lbldegreeResource1"></asp:Label>
      </td>
      <td>
      <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="True" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
              onselectedindexchanged="ddldegree_SelectedIndexChanged" Width="100px" 
              meta:resourcekey="ddldegreeResource1"></asp:DropDownList>
      </td>
      <td>
      <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" 
              Font-Names="Book Antiqua" Font-Size="Medium" 
              meta:resourcekey="lblBranchResource1"></asp:Label>
      </td>
      <td>
      <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
              onselectedindexchanged="ddlbranch_SelectedIndexChanged" Width="150px" 
              meta:resourcekey="ddlbranchResource1"></asp:DropDownList>
      </td>
       <td>
       <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               meta:resourcekey="lblsemResource1"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               onselectedindexchanged="ddlsem_SelectedIndexChanged" Width="50px" 
               AutoPostBack="True" meta:resourcekey="ddlsemResource1"></asp:DropDownList>
       </td>
      <td>
       <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
              meta:resourcekey="lblsecResource1"></asp:Label>
      </td>
      <td>
      <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               onselectedindexchanged="ddlsec_SelectedIndexChanged" Width="50px" 
              meta:resourcekey="ddlsecResource1"></asp:DropDownList>
      </td>
         
        <td>
       <asp:Label ID="lblMonth" runat="server" CssClass="font" Text="Month" 
                meta:resourcekey="lblMonthResource1"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" 
               onselectedindexchanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True" 
               Width="50px" meta:resourcekey="ddlMonthResource1"></asp:DropDownList>
       </td>
       <td>
       <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               meta:resourcekey="lblyearResource1"></asp:Label>
       </td>
        <td>
       <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" 
                onselectedindexchanged="ddlYear_SelectedIndexChanged" AutoPostBack="True"  
                Width="70px" meta:resourcekey="ddlYearResource1"></asp:DropDownList>
       </td> 
       </tr>
      <tr>
       <td>
       <asp:Label ID="lblExamtype" runat="server" Text="Exam Type" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               meta:resourcekey="lblExamtypeResource1" Width="90px"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlexamtype" runat="server" AutoPostBack="True" 
               Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
               onselectedindexchanged="ddlexamtype_SelectedIndexChanged" 
               meta:resourcekey="ddlexamtypeResource1">
           <asp:ListItem meta:resourcekey="ListItemResource1"></asp:ListItem>
           <asp:ListItem meta:resourcekey="ListItemResource2">Theory</asp:ListItem>
           <asp:ListItem meta:resourcekey="ListItemResource3">Practical</asp:ListItem>
           </asp:DropDownList>
       </td>
        <td>
         <asp:Button ID="btnconvert" runat="server" Text="Convert" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnconvert_Click" meta:resourcekey="btnconvertResource1" 
       ></asp:Button>
       </td>
      </tr>
      </table>
      </center>
      
       <br />
    <asp:Label ID="lblerror" runat="server" Text="No Records Found" ForeColor="Red" 
        CssClass="font" meta:resourcekey="lblerrorResource1"></asp:Label>
    <center>
    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="1000">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread></center>
</asp:Content>

