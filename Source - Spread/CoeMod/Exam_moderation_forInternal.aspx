<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Exam_moderation_forInternal.aspx.cs" Inherits="Exam_moderation_forInternal" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="scrptmngr" runat="server"></asp:ScriptManager>

   <%-- <td>
       <asp:TextBox ID="txtFromDate" runat="server" CssClass="font" Width="70px"></asp:TextBox>
       </td>
        <td>
       <asp:Label ID="lblToDate" runat="server" CssClass="font" Text="ToDate"></asp:Label>
       </td>
        <td>
      <asp:TextBox ID="txtToDate" runat="server" CssClass="font"  Width="70px"></asp:TextBox>
       </td>oncellclick="sprdsubjectlist_CellClick" OnPreRender="sprdsubjectlist_SelectedIndexChanged"--%>

<ContentTemplate>
<style type="text/css">
    .font
{
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
       
    }
</style>
<br /><center>
            <asp:Label ID="Label7" runat="server" Text ="Moderation For Internal" 
            Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
       <br /><center>
       <table style="width:700px; height:70px; background-color:#0CA6CA;">
       <tr>
       <td>
       <asp:Label ID="lblMonthandYear" runat="server" CssClass="font" Text="Month and Year" Width="125px"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" 
               onselectedindexchanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
       </td>
        <td>
       <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" 
                onselectedindexchanged="ddlYear_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
       </td>
      <td>
      <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" 
              Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
      </td>
      <td>
      <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="True" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
              onselectedindexchanged="ddldegree_SelectedIndexChanged" Width="100px"></asp:DropDownList>
      </td>
      <td>
      <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" 
              Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
      </td>
      <td>
      <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" 
              Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
              onselectedindexchanged="ddlbranch_SelectedIndexChanged" Width="80px"></asp:DropDownList>
      </td>
       <td>
       <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" ></asp:Label>
       </td>
      <td>
        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" 
               onselectedindexchanged="ddlsem_SelectedIndexChanged"  
               Width="50px"></asp:DropDownList>
       </td>
      <td>
       <asp:Label ID="lblmod" runat="server" Text="Moderation for" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium" Width="125px"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlmodtype" runat="server" AutoPostBack="True" 
               Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
               onselectedindexchanged="ddlmodtype_SelectedIndexChanged">
           <asp:ListItem>Regular</asp:ListItem>
           <asp:ListItem>Arrear</asp:ListItem>
           </asp:DropDownList>
       </td>
       <td>
        <asp:Button ID="btnadd" runat="server" Text="Go" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnadd_Click"></asp:Button>
       </td>
       </tr> 
       
       <tr>
          <td>
      <asp:Label ID="lblapply" runat="server" Text="Apply Moderation Mark" 
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
        ForeColor="Black" Width="180px"></asp:Label>
        </td>
       <td>
        <asp:TextBox ID="TextBox1" runat="server"  Width="37px" Font-Bold="True" 
                   Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="TextBox1" FilterType="Numbers" runat="server"></asp:FilteredTextBoxExtender>
     </td>
      <td>
      <asp:Button ID="btnapply" runat="server" Text="Apply" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnapply_Click"></asp:Button>
      </td>           
       <td>
       <asp:RadioButton ID="RadioBranchwise" text="Branch Wise" GroupName="moderation" 
               Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" 
               runat="server" AutoPostBack="True" Width="120px"></asp:RadioButton>
               </td>
               <td>
        <asp:RadioButton ID="Radiosubjectwise" text="Subject Wise" GroupName="moderation" 
               Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" 
               runat="server" AutoPostBack="True" Width="120px"></asp:RadioButton>
                    
       </td>
       </tr>
       </table></center>
     <br />
       <table><tr><td>
       <asp:Label ID="lblerror" runat="server" Text="No Records Found"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
        ForeColor="Red" Visible="false"></asp:Label>
       </td></tr></table>
       <center>
     <FarPoint:FpSpread ID="sprdremainmark" Width=400 Height=200  Visible="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" runat=server><CommandBar ButtonShadowColor="ControlDark" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight" BackColor="Control">
     </CommandBar><Sheets><FarPoint:SheetView SheetName="Sheet1">
     </FarPoint:SheetView></Sheets></FarPoint:FpSpread>
       </center>
        <center>
       <table><tr><td>
       <farpoint:fpspread ID="sprdsubjectlist" runat="server" Visible="false" >
          
            <commandbar backcolor="Control" buttonfacecolor="Control" 
                buttonhighlightcolor="ControlLightLight" buttonshadowcolor="ControlDark" ButtonType="PushButton">
            </commandbar>
            <sheets>
                <FarPoint:sheetview SheetName="Sheet1">
                </FarPoint:sheetview>
            </sheets>
        </farpoint:fpspread></td></tr></table></center>
         <table>
       <tr>
       <td>
       </td>
       </tr>
       </table>
       <center><table>
        <tr>
        <td>
        <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnsave_Click" 
        Visible="false" ></asp:Button></td>
         <td>
        <asp:Button ID="Btndelete" runat="server" Text="Delete" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" onclick="btndelete_Click" 
        Visible="False"></asp:Button></td>
        </tr>
        </table></center>
       </ContentTemplate> 
</asp:Content>

